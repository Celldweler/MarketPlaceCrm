using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlaceCrm.WebApi.ServicesExtensions
{
    public class WebServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddSignalR();
        }
    }
}