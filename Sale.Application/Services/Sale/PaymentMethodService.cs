using System.Net;
using FluentValidation;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Domain.Responses;

namespace Sale.Application.Services.Sale;

public class PaymentMethodService
{
    private readonly IValidator<PaymentMethodModel> _validator;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    
    public PaymentMethodService(
        IValidator<PaymentMethodModel> validator,
        IPaymentMethodRepository paymentMethodRepository
    )
    {
        _validator = validator;
        _paymentMethodRepository = paymentMethodRepository;
    }
    
    public async Task<Result<PaymentMethodModel>> CreatePaymentMethod(PaymentMethodModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
    
        if (!validationResult.IsValid)
        {
            return Result<PaymentMethodModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest 
            );

        }

        var result = await _paymentMethodRepository.CreateAsync(model);
    
        if (result == null) 
        {
            return Result<PaymentMethodModel>.Failure(
                new List<string> { "No se pudo crear el método de pago." }, 
                HttpStatusCode.InternalServerError 
            );

        }

        return Result<PaymentMethodModel>.Success(result, HttpStatusCode.Created);
    }
    
    public async Task<Result<List<PaymentMethodDto>>> GetAllPaymentMethods()
    {
        var result = await _paymentMethodRepository.GetAll();

        if (result == null || result.Count == 0)
        {
            return Result<List<PaymentMethodDto>>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404) }, 
                HttpStatusCode.NotFound
            );
        }                                                                                                               
        return Result<List<PaymentMethodDto>>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<PaymentMethodModel>> GetPaymentMethodsById(int id)
    {
        var result = await _paymentMethodRepository.GetByIdAsync(id);
        if (result is null)
        {
            return Result<PaymentMethodModel>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404)  }, 
                HttpStatusCode.NotFound
            );
        }
        return Result<PaymentMethodModel>.Success(result, HttpStatusCode.OK);
    }

    public async Task<Result<PaymentMethodModel>> UpdatePaymentMethod(PaymentMethodModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Result<PaymentMethodModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                HttpStatusCode.BadRequest
            );
        }

        var result = await _paymentMethodRepository.UpdateAsync(model);

        if (result == null)
        {
            return Result<PaymentMethodModel>.Failure(
                new List<string> { "No se pucdo actualizar el metodo de pago" },
                HttpStatusCode.InternalServerError
            );
        }

        return Result<PaymentMethodModel>.Success(result, HttpStatusCode.OK);
    }

    public async Task<Result<bool>> DeletePaymentMethod(int id)
    {
        var result = await _paymentMethodRepository.DeleteAsync(id);

        if (!result)
        {
            return Result<bool>.Failure(
                new List<string> { "No se pudo eliminar el metodo de pago" },
                HttpStatusCode.InternalServerError
            );
        }

        return Result<bool>.Success(result, HttpStatusCode.OK);
    }
    
}