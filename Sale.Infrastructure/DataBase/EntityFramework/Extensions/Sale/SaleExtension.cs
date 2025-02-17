using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;

public static class SaleExtension
{
    public static SaleEntity ToEntity(this SaleModel model)
    {
        return new SaleEntity()
        {
            Id = model.Id,
            SaleDate = model.SaleDate,
            Total = model.Total,
            PaymentMethodId = model.PaymentMethodId
        };
    }
    
    public static SaleModel ToModel(this SaleEntity entity)
    {
        return new SaleModel
        (
            entity.Id,
            entity.SaleDate,
            entity.Total,
            entity.PaymentMethodId
        );
    }
    
    public static SaleDto ToSaleDto(this SaleEntity entity)
    {
        return new SaleDto
        (
            entity.Id,
            entity.SaleDate,
            entity.Total,
            entity.PaymentMethodId
            
        );
    }
}