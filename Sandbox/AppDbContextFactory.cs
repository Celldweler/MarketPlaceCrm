using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sandbox;

namespace MarketPlaceCrm.Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SandboxTestDB;Trusted_Connection=True;");
            // optionsBuilder.UseInMemoryDatabase("dev");
            
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}