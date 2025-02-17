using Sale.Application.Services.Sale;
using Sale.Domain.Models.Sale;

namespace Sale.Api.EndPoints.Sale;

public static class DishEndPoints
{
    internal static void MapDishEndPoints(this WebApplication webApp)
    {
        webApp.MapGroup("/dish").WithTags("Dish").MapGroupDish();
    }
    
    internal static void MapGroupDish(this RouteGroupBuilder group)
    {
        group.MapPost("", async (DishModel model, DishService service) =>
        {
            var result = await service.CreateDish(model);
            return result.IsSuccess ? Results.Created($"/dishes/{result.Data.Id}", result.Data) : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapGet("", async (DishService service) =>
        {
            var result = await service.GetAllDishes();
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapGet("/{id:int}", async (int id, DishService service) =>
        {
            var result = await service.GetDishById(id);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapPut("/{id:int}", async (int id, DishModel model, DishService service) =>
        {
            model.Id = id;
            var result = await service.UpdateDish(model);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapDelete("/{id:int}", async (int id, DishService service) =>
        {
            var result = await service.DeleteDish(id);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });
    }
}