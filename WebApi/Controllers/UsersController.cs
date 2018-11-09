using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizModels.Errors;
using BizModels.Identity;
using AutoMapper;
using BusinessServicesContracts.Identity;
using CustomPolicyAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public UsersController(IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            User dbObject = await _userService.Get(_principal, id);

            return Ok(_mapper.Map<UserBizModel>(dbObject));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserBizModel value) {

            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            bool success = await _userService.Update(_principal, _mapper.Map<User>(value), id);

            if (!success) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralUpdateErrorCode,
                    "Update didn't affect any items."));
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralDeleteErrorCode, "Invalid Id."));
            }

            await _userService.SoftDelete(_principal, id);

            return NoContent();
        }
    }
}
