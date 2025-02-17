using Sale.Application.Services.Sale;
using Sale.Domain.Models.Sale;

namespace Sale.Api.EndPoints.Sale;

public static class SaleDetailEndPoints
{
    internal static void MapSaleDetailEndpoints(this WebApplication app)
    {
        app.MapGroup("/sale-details").WithTags("Sale Details").MapGroupSaleDetails();
    }
    
    internal static void MapGroupSaleDetails(this RouteGroupBuilder group)
    {
        group.MapPost("", async (SaleDetailModel model, SaleDetailService service) =>
        {
            var result = await service.CreateSaleDetail(model);
            return result.IsSuccess 
                ? Results.Created($"/sale-details/{result.Data.Id}", result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapGet("", async (SaleDetailService service) =>
        {
            var result = await service.GetAllSaleDetails();
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapGet("/{id:int}", async (int id, SaleDetailService service) =>
        {
            var result = await service.GetSaleDetailById(id);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapPut("/{id:int}", async (int id, SaleDetailModel model, SaleDetailService service) =>
        {
            model.Id = id;
            var result = await service.UpdateSaleDetail(model);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapDelete("/{id:int}", async (int id, SaleDetailService service) =>
        {
            var result = await service.DeleteSaleDetail(id);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });
    }
}