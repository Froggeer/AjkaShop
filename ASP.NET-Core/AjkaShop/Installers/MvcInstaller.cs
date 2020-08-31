using System;
using System.IO;
using System.Reflection;
using Ajka.Common.Constants.Base;
using AjkaShop.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AjkaShop.Installers
{
    public class MvcInstaller : IInstaller
    {
        private const string ApiCorsPolicyDefault = "ApiCorsPolicy";

        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
                var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddCors(options => options.AddPolicy(ApiCorsPolicyDefault, build =>
            {
                build.WithOrigins(BaseConstants.AppLocalhostDevelopmentFrontEndUrl)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials()
                     .SetIsOriginAllowed((host) => true);
            }));
            services.AddMvc();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(BaseConstants.AppStartupSwaggerVersion, new OpenApiInfo
                {
                    Title = BaseConstants.AppStartupApiName,
                    Version = BaseConstants.AppStartupSwaggerVersion,
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
                foreach (var xmlFileName in BaseConstants.AppGeneratedXmlFiles)
                {
                    var xmlFullFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    config.IncludeXmlComments(xmlFullFilePath);
                }
            });
        }
    }
}
