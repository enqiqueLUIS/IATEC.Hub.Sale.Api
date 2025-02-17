using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Repositories.Common;

namespace Sale.Infrastructure.DataBase.EntityFramework.Repositories.Sale;

public class SaleRepository : GenericRepository<SaleEntity>, ISaleRepository
{
    private readonly SaleDbContext _context;
    
    public SaleRepository(SaleDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<SaleModel> CreateAsync(SaleModel model)
    {
        var entity = model.ToEntity();
        var createdEntity = await base.CreateAsync(entity);
        return createdEntity.ToModel();
    }

    public new Task<SaleModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(base.GetByIdAsync(id).Result?.ToModel());
    }

    public async Task<SaleModel> UpdateAsync(SaleModel model)
    {
        var entity = model.ToEntity();
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public Task<List<SaleDto>> GetAll()
    {
        var items = _context.Sales.ToList();
        return Task.FromResult(items.Select(w => w.ToSaleDto()).ToList());
    }
    
    public new async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Sales.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Sales.Remove(entity);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}