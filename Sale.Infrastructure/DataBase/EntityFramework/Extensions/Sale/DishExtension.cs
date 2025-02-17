using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

namespace Sale.Infrastructure.DataBase.EntityFramework.Extensions.Sale;

public static class DishExtension
{
    public static DishEntity ToEntity(this DishModel model)
    {
        return new DishEntity()
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Description = model.Description
        };
    }
    
    public static DishModel ToModel(this DishEntity entity)
    {
        return new DishModel
        (
            entity.Id,
            entity.Name,
            entity.Price,
            entity.Description
        );
    }
    
    public static DishDto ToDishDto(this DishEntity entity)
    {
        return new DishDto
        (
            entity.Id,
            entity.Name,
            entity.Price,
            entity.Description
        );
    }
}