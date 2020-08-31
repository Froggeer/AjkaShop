using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AjkaShop.Installers
{
    public interface IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}
