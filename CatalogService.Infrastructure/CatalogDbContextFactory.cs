using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure;

public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] arges)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CatalogService.API");
        // show the config route
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "CatalogService.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("postgres"));

        return new CatalogDbContext(optionsBuilder.Options);
    }
}
