using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Models.Identity;
using CustomPolicyAuth;
using RepositoryContracts.UnitsOfWork;
using BizModels.Identity;

namespace WebApi.Controllers {

    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/token")]
    public class TokenController : Controller {

        private readonly IIdentityUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Request constants following the https://tools.ietf.org/html/rfc6749
        /// </summary>
        private const string OAuthBadRequest = "invalid_request";
        private const string OAuthInvalidGrant = "invalid_grant";
        private const string OAuthPassword = "password";
        private const string OAuthUsername = "username";
        private const string OAuthGrantType = "grant_type";
        private const string OAuthRefreshTokenGrant = "refresh_token";

        /// <summary>
        /// The default access token expiration timespan in seconds.
        /// Used if no value is found in the config.
        /// </summary>
        private const uint DefaultAccessTokenExpirationSecs = 300;
        /// <summary>
        /// The default refresh token expiration timespan in seconds.
        /// Used if no value is found in the config.
        /// </summary>
        private const uint DefaultRefreshTokenExpirationSecs = 3600;

        public TokenController(IIdentityUnitOfWork unitOfWork, UserManager<User> userManager) {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        /// <summary>
        /// Access token endpoint for grant_type password following the https://tools.ietf.org/html/rfc6749
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> IssueAccessToken([FromForm]IFormCollection form) {

            if (form.Count <= 0) {
                return BadRequest(OAuthBadRequest);
            }

            try {
                if (form[OAuthGrantType].Equals(OAuthPassword)) {
                    return await HandlePasswordGrant(form[OAuthUsername], form[OAuthPassword]);
                }
                if (form[OAuthGrantType].Equals(OAuthRefreshTokenGrant)) {
                    return await HandleRefreshTokenGrant(form[OAuthRefreshTokenGrant]);
                }
            }
            catch (Exception) {
                return BadRequest(OAuthBadRequest);
            }

            return BadRequest(OAuthBadRequest);
        }

        /// <summary>
        /// Endpoint to invalidate refresh token. This should be done on logout.
        /// </summary>
        [HttpDelete("{refreshToken}")]
        public async Task<IActionResult> RevokeRefreshToken(string refreshToken) {

            if (string.IsNullOrWhiteSpace(refreshToken)) {
                return BadRequest(OAuthBadRequest);
            }

            try {
                ContextPrincipal principal = new ContextPrincipal(HttpContext.User);

                RefreshToken token = await unitOfWork.TokenRepository.FindOneAsync(
                    t => t.UserId.Equals(principal.UserId) && t.RefreshTokenValue.Equals(refreshToken), o => o.OrderBy(u => u.Id), null
                );
                if (null != token) {
                    unitOfWork.TokenRepository.Delete(token);
                    await unitOfWork.Save();
                    return Ok();
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception) {
                return BadRequest(OAuthBadRequest);
            }
        }

        /// <summary>
        /// Gets the claims encoded in the current token and returns them as key, value.
        /// Note that authorization is required for this endpoint.
        /// </summary>
        /// <returns>Dictionary<string, string> with the claims in it.</returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        public IActionResult TokenInfo() {

            ContextPrincipal principal = new ContextPrincipal(HttpContext.User);

            return Ok(principal.Claims.ToDictionary(cl => cl.Type, cl => cl.Value));
        }

        private async Task<IActionResult> HandlePasswordGrant(string email, string password) {

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException(@"Username or password is invalid!");
            }

            // TODO: This is breaking our DAL. However, it is exposed by MS. I'm not sure if we're supposed to hide it.
            User user = await userManager.Users.Include(u => u.RefreshToken).FirstOrDefaultAsync(u => u.Email.Equals(email));
            if (user == null) {
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            bool validCredentials = await userManager.CheckPasswordAsync(user, password);
            if (!validCredentials) {
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            Guid userId = user.Id;
            if (userId.Equals(Guid.Empty)) {
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            if (user.RefreshToken != null) {
                unitOfWork.TokenRepository.Delete(user.RefreshToken);
            }

            RefreshToken newRefreshToken = GenerateRefreshToken(user);

            unitOfWork.TokenRepository.Insert(newRefreshToken);

            await unitOfWork.Save();

            HashSet<Claim> claims = GenerateClaims(user);

            return Ok(GenerateAccessToken(claims, newRefreshToken.RefreshTokenValue));
        }

        private async Task<IActionResult> HandleRefreshTokenGrant(string refreshToken) {

            if (string.IsNullOrWhiteSpace(refreshToken)) {
                throw new ArgumentException(@"Refresh token is invalid!");
            }

            RefreshToken existingRefreshToken = await unitOfWork.TokenRepository.
                FindOneAsync(t => t.RefreshTokenValue.Equals(refreshToken), o => o.OrderBy(u => u.Id), null);

            if (null == existingRefreshToken) {
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            if (existingRefreshToken.ExpiresUtc <= DateTime.UtcNow) {
                unitOfWork.TokenRepository.DeleteById(existingRefreshToken.Id);
                await unitOfWork.Save();
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            User user = await userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
            if (null == user) {
                return StatusCode((int)HttpStatusCode.Unauthorized, OAuthInvalidGrant);
            }

            unitOfWork.TokenRepository.Delete(existingRefreshToken);

            RefreshToken newRefreshToken = GenerateRefreshToken(user);

            unitOfWork.TokenRepository.Insert(newRefreshToken);

            await unitOfWork.Save();

            HashSet<Claim> claims = GenerateClaims(user);

            return Ok(GenerateAccessToken(claims, newRefreshToken.RefreshTokenValue));
        }

        private TokenResponse GenerateAccessToken(IEnumerable<Claim> claims, string refreshToken) {

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("AudienceSigningKey")));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            DateTime now = DateTime.UtcNow;
            uint accessTokenExpiration = ConfigUtils.ConfigurationProvider.GetDefaultConfig().
                GetSection("AuthTokens").GetValue<uint>("AccessTokenExpireTimeSecs");
            if (accessTokenExpiration <= 0) {
                accessTokenExpiration = DefaultAccessTokenExpirationSecs;
            }

            JwtSecurityToken token = new JwtSecurityToken(
                ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("Issuer"),
                ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("Audience"),
                claims,
                DateTime.UtcNow,
                now.Add(TimeSpan.FromSeconds(accessTokenExpiration)),
                credentials
            );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string accessToken = handler.WriteToken(token);

            TokenResponse response = new TokenResponse {
                AccessToken = accessToken,
                ExpiresIn = (int)accessTokenExpiration,
                RefreshToken = refreshToken,
                TokenType = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant()
            };

            return response;
        }

        private HashSet<Claim> GenerateClaims(User user) {

            HashSet<Claim> claims = new HashSet<Claim>();

            if (null == user) {
                return claims;
            }

            claims.Add(new Claim(MaMClaimTypes.Address, user.Address ?? string.Empty));
            claims.Add(new Claim(MaMClaimTypes.Email, user.Email ?? string.Empty));
            claims.Add(new Claim(MaMClaimTypes.Name, user.UserName ?? string.Empty));
            claims.Add(new Claim(MaMClaimTypes.PhoneNumber, user.PhoneNumber ?? string.Empty));
            claims.Add(new Claim(MaMClaimTypes.UserId, user.Id.ToString() ?? string.Empty));
            // Maybe we can add the "assocEntity => RoleId" as claims here
            return claims;
        }
        /// <summary>
        /// Generates new refresh token WITHOUT DB persistence!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private RefreshToken GenerateRefreshToken(User user) {

            DateTime now = DateTime.UtcNow;
            uint refreshTokenExpiration = ConfigUtils.ConfigurationProvider.GetDefaultConfig().
                GetSection("AuthTokens").GetValue<uint>("RefreshTokenExpireTimeSecs");
            if (refreshTokenExpiration <= 0) {
                refreshTokenExpiration = DefaultRefreshTokenExpirationSecs;
            }

            RefreshToken refreshTokenModel = new RefreshToken {
                RefreshTokenValue = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"),
                IssuedUtc = now,
                ExpiresUtc = now.Add(TimeSpan.FromSeconds(refreshTokenExpiration)),
                UserId = user.Id
            };

            return refreshTokenModel;
        }
    }
}
