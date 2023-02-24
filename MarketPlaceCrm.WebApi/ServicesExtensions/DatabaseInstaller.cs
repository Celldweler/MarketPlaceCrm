using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Services.OrderServices;
using MarketPlaceCrm.WebApi.Services;
using MarketPlaceCrm.WebApi.Services.FakeStores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlaceCrm.WebApi.ServicesExtensions
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<FakeProductStore>();
            services.AddTransient<JwtManager>();
            services.AddTransient<NotificationsService>();

            services.AddTransient<OrderHistoryService>();

            services.AddDbContext<AppDbContext>();
        }
    }
}