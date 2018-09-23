using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts.UnitsOfWork;
using Repository.UnitsOfWork;
using RepositoryContracts;
using Microsoft.AspNetCore.Authorization;
using CustomPolicyAuth;
using Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repository.Base;
using Repository.Identity;
using RepositoryContracts.Identity;
using RepositoryContracts.Base;

namespace WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPool<Context>(options => options//.UseInMemoryDatabase());
            .UseMySql(
                     ConfigUtils.ConfigurationProvider
                    .GetDefaultConfig()
                    .GetSection("ConnectionStrings").GetValue<string>("MainDB")
                ), 128);

            services.AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddTransient<IEntityAncestorRepository, EntityAncestorRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();


            //services.AddTransient<IValidator, Validator>();
            //services.AddTransient<IMailUtility, MailUtility>();
            //services.AddTransient<IGlobalExceptionHandlingDelegate, GlobalExceptionHandlingDelegate>();

            //services.AddTransient<CustomSignInManager>();

            services.AddTransient<IAuthorizationHandler, AuthHandler>();

            services.AddIdentity<User, Role>(options => {
                //TODO: Retrieve these values from config
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();

            services
                .AddAuthentication(options => {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("Issuer"),
                        ValidAudience = ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                ConfigUtils.ConfigurationProvider.GetDefaultConfig().GetSection("AuthTokens").GetValue<string>("AudienceSigningKey")
                            )
                        )
                    };
                }
            );

            services.AddAuthorization(options => {
                options.AddPolicy("CustomPolicy", policy =>
                    policy.Requirements.Add(new CustomAuthRequirement()));
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
