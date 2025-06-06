using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace PrepPal.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PrepPalDbContext>
{
    public PrepPalDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<PrepPalDbContext>();
        var connectionString = "Host=localhost;Database=RecipeDB;Username=postgres;Password=1801810b";

        builder.UseNpgsql(connectionString);

        return new PrepPalDbContext(builder.Options);
    }
}