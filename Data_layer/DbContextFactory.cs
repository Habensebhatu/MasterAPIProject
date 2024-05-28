using Data_layer.Context.Data;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

public interface IDbContextFactory
{
    DbContext CreateDbContext(string connectionStringName);
}

public class DbContextFactory : IDbContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public DbContext CreateDbContext(string connectionStringName)
    {
        var connectionString = _configuration.GetConnectionString(connectionStringName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found.");
        }

        switch (connectionStringName)
        {
            case "Aitmaten":
                var optionsBuilderAitmaten = new DbContextOptionsBuilder<MyDbContext>();
                optionsBuilderAitmaten.UseSqlServer(connectionString);
                return new MyDbContext(optionsBuilderAitmaten.Options);

            case "SofaniMarket":
                var optionsBuilderSofani = new DbContextOptionsBuilder<MyDbcontextSofani>();
                optionsBuilderSofani.UseSqlServer(connectionString);
                return new MyDbcontextSofani(optionsBuilderSofani.Options);

            default:
                throw new ArgumentException("Invalid connection string name", nameof(connectionStringName));
        }
    }
}
