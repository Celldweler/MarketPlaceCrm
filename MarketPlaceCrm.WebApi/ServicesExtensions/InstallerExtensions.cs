using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlaceCrm.WebApi.ServicesExtensions
{
    public static class InstallerExtensions 
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installersTypes = typeof(Startup).Assembly.ExportedTypes;
            
            var installers = installersTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x)
                            && !x.IsAbstract
                            && !x.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
            
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}