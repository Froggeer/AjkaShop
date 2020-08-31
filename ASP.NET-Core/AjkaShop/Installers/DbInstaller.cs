using Ajka.Common.Constants.Base;
using Ajka.DAL;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AjkaShop.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AjkaShopDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(BaseConstants.DbDefaultConnectionIdentifier)));
            services.AddScoped<IAjkaShopDbContext>(provider => provider.GetService<AjkaShopDbContext>());
            services.AddUnitOfWork<AjkaShopDbContext>();
        }
    }
}
