using System.Net;
using FluentValidation;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Domain.Responses;

namespace Sale.Application.Services.Sale;

public class SaleService
{
    private readonly IValidator<SaleModel> _validator;
    private readonly ISaleRepository _saleRepository;
    
    public SaleService(
        IValidator<SaleModel> validator,
        ISaleRepository saleRepository
    )
    {
        _validator = validator;
        _saleRepository = saleRepository;
    }
    
    public async Task<Result<SaleModel>> CreateSale(SaleModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
    
        if (!validationResult.IsValid)
        {
            return Result<SaleModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest 
            );

        }

        var result = await _saleRepository.CreateAsync(model);
    
        if (result == null) 
        {
            return Result<SaleModel>.Failure(
                new List<string> { "No se pudo crear la venta." }, 
                HttpStatusCode.InternalServerError 
            );

        }

        return Result<SaleModel>.Success(result, HttpStatusCode.Created);
    }

    public async Task<Result<List<SaleDto>>> GetAllSales()
    {
        var result = await _saleRepository.GetAll();

        if (result == null || result.Count == 0)
        {
            return Result<List<SaleDto>>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404) },
                HttpStatusCode.NotFound
            );
        }
        
        return Result<List<SaleDto>>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<SaleModel>> GetSaleById(int id)
    {
        var result = await _saleRepository.GetByIdAsync(id);

        if (result == null)
        {
            return Result<SaleModel>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404) },
                HttpStatusCode.NotFound
            );
        }

        return Result<SaleModel>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<SaleModel>> UpdateSale(SaleModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Result<SaleModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                HttpStatusCode.BadRequest
            );
        }

        var result = await _saleRepository.UpdateAsync(model);

        if (result == null)
        {
            return Result<SaleModel>.Failure(
                new List<string> { "No se pudo actualizar la venta." },
                HttpStatusCode.InternalServerError
            );
        }

        return Result<SaleModel>.Success(result, HttpStatusCode.OK);
    }

    public async Task<Result<bool>> DeleteSale(int id)
    {
        var result = await _saleRepository.DeleteAsync(id);

        if (!result)
        {
            return Result<bool>.Failure(
                new List<string> { "No se pudo eliminar la venta." },
                HttpStatusCode.InternalServerError
            );
        }

        return Result<bool>.Success(result, HttpStatusCode.OK);
    }
}