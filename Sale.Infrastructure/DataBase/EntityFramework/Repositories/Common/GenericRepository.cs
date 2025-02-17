using Microsoft.EntityFrameworkCore;
using Sale.Domain.Repositories.Common;
using Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Repositories.Common;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly SaleDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(SaleDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _dbSet.FindAsync(id);
        if (result is null)
        {
            return false;
        }
        _context.Remove(result);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entityEntry = _dbSet.Update(entity);
        _context.SaveChanges();
        return Task.FromResult(entityEntry.Entity);
    }
}