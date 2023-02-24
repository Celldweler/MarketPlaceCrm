using System.Linq;
using MarketPlaceCrm.Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketPlaceCrm.Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            if (args != null && args.Length > 0)
            {
                if (args[0] == "dev")
                {
                    optionsBuilder.UseInMemoryDatabase("Dev");

                    return new AppDbContext(optionsBuilder.Options);
                }
            }
            optionsBuilder.UseSqlServer(Settings.ConnectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}