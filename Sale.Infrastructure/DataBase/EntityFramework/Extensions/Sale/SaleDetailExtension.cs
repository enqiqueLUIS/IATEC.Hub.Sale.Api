using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;

public static class SaleDetailExtension
{
    public static SaleDetailEntity ToEntity(this SaleDetailModel model)
    {
        return new SaleDetailEntity()
        {
            Id = model.Id,
            SaleId = model.SaleId,
            DishId = model.DishId,
            Amount = model.Amount,
            SubTotal = model.SubTotal
        };
    }
    
    public static SaleDetailModel ToModel(this SaleDetailEntity entity)
    {
        return new SaleDetailModel
        (
            entity.Id,
            entity.SaleId,
            entity.DishId,
            entity.Amount,
            entity.SubTotal
        );
    }
    
    public static SaleDetailDto ToSaleDetailDto(this SaleDetailEntity entity)
    {
        return new SaleDetailDto
        (
            entity.Id,
            entity.SaleId,
            entity.DishId,
            entity.Amount,
            entity.SubTotal
        );
    }
}