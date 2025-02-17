using Sale.Application.Services.Sale;
using Sale.Domain.Models.Sale;

namespace Sale.Api.EndPoints.Sale;

public static class SaleEndpoints
{
    internal static void MapSaleEndpoints(this WebApplication app)
    {
        app.MapGroup("/sales").WithTags("Sales").MapGroupSales();
    }
    
    internal static void MapGroupSales(this RouteGroupBuilder group)
    {
        group.MapPost("", async (SaleModel model, SaleService service) =>
        {
            var result = await service.CreateSale(model);
            return result.IsSuccess 
                ? Results.Created($"/sales/{result.Data.Id}", result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapGet("", async (SaleService service) =>
        {
            var result = await service.GetAllSales();
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapGet("/{id:int}", async (int id, SaleService service) =>
        {
            var result = await service.GetSaleById(id);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapPut("/{id:int}", async (int id, SaleModel model, SaleService service) =>
        {
            model.Id = id;
            var result = await service.UpdateSale(model);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapDelete("/{id:int}", async (int id, SaleService service) =>
        {
            var result = await service.DeleteSale(id);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });
    }
}
