using System.Net;
using FluentValidation;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Domain.Responses;

namespace Sale.Application.Services.Sale;

public class SaleDetailService
{
    private readonly IValidator<SaleDetailModel> _validator;
    private readonly ISaleDetailRepository _saleDetailRepository;
    
    public SaleDetailService(
        IValidator<SaleDetailModel> validator,
        ISaleDetailRepository saleDetailRepository
    )
    {
        _validator = validator;
        _saleDetailRepository = saleDetailRepository;
    }
    
    public async Task<Result<SaleDetailModel>> CreateSaleDetail(SaleDetailModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
    
        if (!validationResult.IsValid)
        {
            return Result<SaleDetailModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest 
            );

        }

        var result = await _saleDetailRepository.CreateAsync(model);
    
        if (result == null) 
        {
            return Result<SaleDetailModel>.Failure(
                new List<string> { "No se pudo crear el detalle de venta." }, 
                HttpStatusCode.InternalServerError 
            );

        }

        return Result<SaleDetailModel>.Success(result, HttpStatusCode.Created);
    }
    
    public async Task<Result<List<SaleDetailDto>>> GetAllSaleDetails()
    {
        var saleDetails = await _saleDetailRepository.GetAll();
        
        var saleDetailsDto = saleDetails.Select(s => 
            new SaleDetailDto(s.Id, s.SaleId, s.DishId, s.Amount, s.SubTotal)
        ).ToList();

        return Result<List<SaleDetailDto>>.Success(saleDetailsDto, HttpStatusCode.OK);
    }

    
    public async Task<Result<SaleDetailModel>> GetSaleDetailById(int id)
    {
        var result = await _saleDetailRepository.GetByIdAsync(id);

        if (result == null)
        {
            return Result<SaleDetailModel>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404)}, 
                HttpStatusCode.NotFound
            );
        }

        return Result<SaleDetailModel>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<SaleDetailModel>> UpdateSaleDetail(SaleDetailModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Result<SaleDetailModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest
            );
        }

        var result = await _saleDetailRepository.UpdateAsync(model);

        if (result == null)
        {
            return Result<SaleDetailModel>.Failure(
                new List<string> { "No se pudo actualizar el detalle de venta." }, 
                HttpStatusCode.InternalServerError
            );
        }

        return Result<SaleDetailModel>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<bool>> DeleteSaleDetail(int id)
    {
        var result = await _saleDetailRepository.DeleteAsync(id);
        if (!result)
        {
            return Result<bool>.Failure(
                new List<string> { "Detalle de venta no encontrado." }, 
                HttpStatusCode.InternalServerError
            );
        }
        return Result<bool>.Success(result, HttpStatusCode.OK);
    }
}