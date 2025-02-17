using Sale.Application.Services.Sale;
using Sale.Domain.Models.Sale;

namespace Sale.Api.EndPoints.Sale;

public static class PaymentMethodEndPoints
{
    internal static void MapPaymentMethodEndpoints(this WebApplication app)
    {
        app.MapGroup("/payment-methods").WithTags("Payment Methods").MapGroupPaymentMethods();
    }
    
    internal static void MapGroupPaymentMethods(this RouteGroupBuilder group)
    {
        group.MapPost("", async (PaymentMethodModel model, PaymentMethodService service) =>
        {
            var result = await service.CreatePaymentMethod(model);
            return result.IsSuccess 
                ? Results.Created($"/payment-methods/{result.Data.Id}", result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapGet("", async (PaymentMethodService service) =>
        {
            var result = await service.GetAllPaymentMethods();
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapGet("/{id:int}", async (int id, PaymentMethodService service) =>
        {
            var result = await service.GetPaymentMethodsById(id);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Errors);
        });

        group.MapPut("/{id:int}", async (int id, PaymentMethodModel model, PaymentMethodService service) =>
        {
            model.Id = id;
            var result = await service.UpdatePaymentMethod(model);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });

        group.MapDelete("/{id:int}", async (int id, PaymentMethodService service) =>
        {
            var result = await service.DeletePaymentMethod(id);
            return result.IsSuccess 
                ? Results.Ok(result.Data) 
                : Results.Problem(string.Join(", ", result.Errors), statusCode: (int)result.StatusCode);
        });
    }
}