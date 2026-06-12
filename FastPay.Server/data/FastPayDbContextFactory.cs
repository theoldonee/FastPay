using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastPay.Server.Data;

public sealed class FastPayDbContextFactory : IDesignTimeDbContextFactory<FastPayDbContext>
{
    public FastPayDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<FastPayDbContextFactory>()
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("FastPayDb")
            ?? throw new InvalidOperationException("Connection String 'FastPayDb' was not found");

        var options = new DbContextOptionsBuilder<FastPayDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new FastPayDbContext(options);
    }
}
