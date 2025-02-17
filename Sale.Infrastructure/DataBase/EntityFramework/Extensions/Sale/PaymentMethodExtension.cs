using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;

public static class PaymentMethodExtension
{
    public static PaymentMethodEntity ToEntity(this PaymentMethodModel model)
    {
        return new PaymentMethodEntity()
        {
            Id = model.Id,
            Name = model.Name
        };
    }
    
    public static PaymentMethodModel ToModel(this PaymentMethodEntity entity)
    {
        return new PaymentMethodModel
        (
            entity.Id,
            entity.Name
        );
    }
    
    public static PaymentMethodDto ToPaymentMethodDto(this PaymentMethodEntity entity)
    {
        return new PaymentMethodDto
        (
            entity.Id,
            entity.Name
        );
    }
}