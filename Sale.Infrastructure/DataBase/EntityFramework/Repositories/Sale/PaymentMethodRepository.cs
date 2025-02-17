using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Context.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions;
using Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Repositories.Common;

namespace Sale.Infrastructure.DataBase.EntityFramework.Repositories.Sale;

public class PaymentMethodRepository : GenericRepository<PaymentMethodEntity>, IPaymentMethodRepository
{
    private readonly SaleDbContext _context;

    public PaymentMethodRepository(SaleDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaymentMethodModel> CreateAsync(PaymentMethodModel model)
    {
        var entity = model.ToEntity();
        var createdEntity = await base.CreateAsync(entity);
        return createdEntity.ToModel();
    }

    public new Task<PaymentMethodModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(base.GetByIdAsync(id).Result?.ToModel());
    }

    public async Task<PaymentMethodModel> UpdateAsync(PaymentMethodModel model)
    {
        var entity = model.ToEntity();
        var updatedEntity = await base.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.ToModel();
    }

    public async Task<List<PaymentMethodDto>> GetAll()
    {
        var items = await _context.PaymentMethods.ToListAsync();
        return items.Select(w => w.ToPaymentMethodDto()).ToList();
    }

    public new async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.PaymentMethods.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.PaymentMethods.Remove(entity);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}