using System;
using Finance.Business.Services;
using Finance.Web.Api.Configuration;
using Finance.Web.Api.Filters;
using Finance.Web.Api.Services;
using Finance.Web.Api.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Finance.Core.Extensions;
using Finance.Web.Api.Models;
using ConfigurationConstants = Finance.Web.Api.Configuration.ConfigurationConstants;

namespace Finance.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenConfiguration =
                Configuration.GetRequired<TokenConfiguration>(ConfigurationConstants.TokenConfiguration);
            services.AddSingleton(tokenConfiguration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = tokenConfiguration.Access.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = tokenConfiguration.Access.SecurityKey
                    };
                });

            services.AddAuthorization();
            //services.AddSingleton<IAuthorizationHandler, HttpAuthorizationHandler>();

            Data.Bootstrapper.ConfigureServices(Configuration, services);
            Business.Bootstrapper.ConfigureServices(Configuration, services);

            services
                .AddScoped<IAuthenticationService<GoogleAuthenticationData>, GoogleAuthenticationService>()
                .AddScoped(UserAuthenticationServiceFactory)
                .AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>(); ;

            services.AddControllers(options => options.Filters.Add(typeof(ModelValidationActionFilter)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .UseStaticFiles();
        }

        private static IAuthenticationService<UserAuthenticationData> UserAuthenticationServiceFactory(IServiceProvider services)
        {
            var tokenConfiguration = services.GetRequiredService<TokenConfiguration>();
            var securityTokenHandler = services.GetRequiredService<SecurityTokenHandler>();

            var accessTokenGenerator = new JwtTokenGenerator(tokenConfiguration.Access, securityTokenHandler);
            var refreshTokenGenerator = new JwtTokenGenerator(tokenConfiguration.Refresh, securityTokenHandler);

            var userLoginService = services.GetRequiredService<IUserLoginService>();
            var userService = services.GetRequiredService<IUserService>();

            return new UserAuthenticationService(userLoginService, userService, accessTokenGenerator,
                refreshTokenGenerator);
        }
    }
}
