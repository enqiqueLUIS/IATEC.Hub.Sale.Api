using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;

public class SaleDbContextFactory : IDesignTimeDbContextFactory<SaleDbContext>
{
    public SaleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SaleDbContext>();
        
        var connectionString = "Server=MSI\\SQLEXPRESS;Database=IATECSale;Trusted_Connection=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);

        return new SaleDbContext(optionsBuilder.Options);
    }
}