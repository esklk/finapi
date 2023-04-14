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
                    x.TokenValidationParameters = BuildJwtValidationParameters(tokenConfiguration.Access);
                });

            services.AddAuthorization();

            Data.Bootstrapper.ConfigureServices(Configuration, services);
            Business.Bootstrapper.ConfigureServices(Configuration, services);

            services
                .AddScoped<IAuthenticationService<GoogleAuthenticationData>, GoogleAuthenticationService>()
                .AddScoped<IAuthenticationService<UserAuthenticationData>>(UserAuthenticationServiceFactory)
                .AddScoped<IAuthenticationService<AuthModel>>(RefreshAuthenticationServiceFactory);

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ModelValidationActionFilter));
                options.Filters.Add(typeof(HttpExceptionFilter));
            });
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

        private static TokenValidationParameters BuildJwtValidationParameters(JwtConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = configuration.Issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = configuration.SecurityKey
            };
        }

        private static UserAuthenticationService UserAuthenticationServiceFactory(IServiceProvider services)
        {
            var tokenConfiguration = services.GetRequiredService<TokenConfiguration>();
            var accessTokenGenerator = new JwtTokenGenerator(tokenConfiguration.Access);
            var refreshTokenGenerator = new JwtTokenGenerator(tokenConfiguration.Refresh);

            var userLoginService = services.GetRequiredService<IUserLoginService>();
            var userService = services.GetRequiredService<IUserService>();

            return new UserAuthenticationService(userLoginService, userService, accessTokenGenerator,
                refreshTokenGenerator);
        }

        private static RefreshAuthenticationService RefreshAuthenticationServiceFactory(IServiceProvider services)
        {
            var tokenConfiguration = services.GetRequiredService<TokenConfiguration>();
            var accessTokenGenerator = new JwtTokenGenerator(tokenConfiguration.Access);
            var accessTokenValidator = new JwtTokenValidator(BuildJwtValidationParameters(tokenConfiguration.Access));
            var refreshTokenValidator = new JwtTokenValidator(BuildJwtValidationParameters(tokenConfiguration.Refresh));

            return new RefreshAuthenticationService(accessTokenGenerator, accessTokenValidator, refreshTokenValidator);
        }
    }
}
