using System;
using System.Linq;
using Ajka.BL;
using Ajka.Common.Constants.Base;
using AjkaShop.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AjkaShop
{
    public class Startup
    {
        private const string ApiCorsPolicyDefault = "ApiCorsPolicy";

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            var serviceBootstrapper = new Bootstrapper();
            serviceBootstrapper.RegisterServices(services);

            var serviceInstallers = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            serviceInstallers.ForEach(installer => installer.InstallService(services, Configuration));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseFileServer();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint($"/swagger/{BaseConstants.AppStartupSwaggerVersion}/swagger.json",
                    $"{BaseConstants.AppStartupApiName} {BaseConstants.AppStartupSwaggerVersion}");
            });

            app.UseRouting();

            app.UseCors(ApiCorsPolicyDefault);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
