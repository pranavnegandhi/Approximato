using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Notadesigner.Approximato.Data;

public class ApproximatoDbContextFactory : IDesignTimeDbContextFactory<ApproximatoDbContext>
{
    public ApproximatoDbContext CreateDbContext(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
        var configuration = configurationBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApproximatoDbContext>()
            .UseSqlite(configuration.GetConnectionString("PrimaryStorage"));

        return new ApproximatoDbContext(optionsBuilder.Options);
    }
}