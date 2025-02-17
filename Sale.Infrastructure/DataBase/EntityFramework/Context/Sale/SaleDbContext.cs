using Microsoft.EntityFrameworkCore;
using Sale.Infrastructure.DataBase.EntityFramework.Entities;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;

public class SaleDbContext : DbContext
{
    public DbSet<DishEntity> Dishes { get; set; }
    public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
    public DbSet<SaleDetailEntity> SaleDetails { get; set; }
    public DbSet<SaleEntity> Sales { get; set; }
    
    
    public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options) {}
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateAuditFields()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy= GetCurrentUserId();
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = 104;
                    break;
            }
        }
    }
    
    private int GetCurrentUserId()
    {
        return 123;
    }
}