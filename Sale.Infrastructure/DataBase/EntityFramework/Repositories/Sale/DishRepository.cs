using Microsoft.EntityFrameworkCore;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Repositories.Common;

namespace Sale.Infrastructure.DataBase.EntityFramework.Repositories.Sale;

public class DishRepository : GenericRepository<DishEntity>, IDishRepository
{
    private readonly SaleDbContext _context;
    
    public DishRepository(SaleDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<DishModel> CreateAsync(DishModel model)
    {
        var entity = model.ToEntity();
        var createdEntity = await base.CreateAsync(entity);
        return createdEntity.ToModel();
    }
    
    public async Task<List<DishDto>> GetAll()
    {
        var items = await _context.Dishes.ToListAsync();
        return items.Select(w => w.ToDishDto()).ToList();
    }

    public new Task<DishModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(base.GetByIdAsync(id).Result?.ToModel());
    }

    public async Task<DishModel> UpdateAsync(DishModel model)
    {
        var entity = model.ToEntity();
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }
    
    public new async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Dishes.FindAsync(id);
        if (entity is null)
        {
            return false;
        }
        
        _context.Dishes.Remove(entity);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}