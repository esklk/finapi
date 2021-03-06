using Finance.Business.Mapping;
using Finance.Business.Services;
using Finance.Business.Services.Implementation;
using Finance.Core.Configuration;
using Finance.Core.Configuration.Models;
using Finance.Data;
using Finance.Web.Api.Authorization.Handlers;
using Finance.Web.Api.Configuration;
using Finance.Web.Api.Configuration.Implementation;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Filters;
using Finance.Web.Api.Services;
using Finance.Web.Api.Services.Implementation;
using Finance.Web.Api.Services.Tokens.PayloadMapping;
using Finance.Web.Api.Services.Tokens.PayloadMapping.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

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
            JwtConfiguration jwtConfig = Configuration.GetJwtConfiguration();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtConfig.SecurityKey
                    };
                });

            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, HttpAuthorizationHandler>();

            services.AddSingleton<IJwtConfiguration>(jwtConfig);

            DatabaseConfiguration dbConfig = Configuration.GetDatabaseConfiguration("FinaApiDb");
            services.AddDbContext<FinApiDbContext>(x => x.UseMySql(dbConfig.BuildConnectionString(), new MySqlServerVersion(dbConfig.ServerVersion)));

            services.AddAutoMapper(typeof(DefaultMappingProfile));

            services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IOperationCategoryService, OperationCategoryService>()
                .AddScoped<IOperationService, OperationService>()
                .AddScoped<IUserService, UserService>();

            Dictionary<string, OAuthConfiguration> oauthConfigs = Configuration.GetConfigurationDictionary<OAuthConfiguration>(Api.Configuration.ConfigurationConstants.OAuthConfiguration);
            services.AddSingleton<IDictionary<string, OAuthConfiguration>>(oauthConfigs);

            services.AddHttpClient();

            services
                .AddSingleton<GoogleAccessTokenConfiguration>()
                .AddSingleton<GoogleAccessTokenValidator>()
                .AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddSingleton<IJwtTokenManager, JwtTokenManager>()
                .AddScoped<ILoginService, LoginService>()
                .AddSingleton<IOauthConfigurationProvider, OauthConfigurationProvider>()
                .AddSingleton<IPayloadMapperFactory, AccessTokenPayloadMapperFactory>()
                .AddSingleton<ITokenValidatorFactory, OAuthTokenValidatorFactory>()
                .AddSingleton<IUriManager, UriManager>()
                .AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

            services.AddControllers(options => options.Filters.Add(typeof(ModelValidationActionFilter)));
            services.AddControllersWithViews();
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
                });
        }
    }
}
