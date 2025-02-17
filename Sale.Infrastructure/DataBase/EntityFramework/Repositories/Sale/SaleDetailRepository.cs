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

public class SaleDetailRepository : GenericRepository<SaleDetailEntity>, ISaleDetailRepository
{
    private readonly SaleDbContext _context;
    
    public SaleDetailRepository(SaleDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<SaleDetailModel> CreateAsync(SaleDetailModel model)
    {
        var entity = model.ToEntity();
        var createdEntity = await base.CreateAsync(entity);
        return createdEntity.ToModel();
    }

    public new Task<SaleDetailModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(base.GetByIdAsync(id).Result?.ToModel());
    }

    public async Task<SaleDetailModel> UpdateAsync(SaleDetailModel model)
    {
        var entity = model.ToEntity();
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public async Task<List<SaleDetailDto>> GetAll()
    {
        var items = await _context.SaleDetails.ToListAsync();
        return items.Select(w => w.ToSaleDetailDto()).ToList();
    }
    
    public new async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.SaleDetails.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.SaleDetails.Remove(entity);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}