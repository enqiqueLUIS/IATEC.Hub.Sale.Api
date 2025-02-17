using System.Net;
using FluentValidation;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;
using Sale.Domain.Responses;

namespace Sale.Application.Services.Sale;

public class DishService
{
    private readonly IValidator<DishModel> _validator;
    private readonly IDishRepository _dishRepository;
    
    public DishService(
        IValidator<DishModel> validator,
        IDishRepository dishRepository
    )
    {
        _validator = validator;
        _dishRepository = dishRepository;
    }
    
    public async Task<Result<DishModel>> CreateDish(DishModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
    
        if (!validationResult.IsValid)
        {
            return Result<DishModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest 
            );

        }

        var result = await _dishRepository.CreateAsync(model);
    
        if (result == null) 
        {
            return Result<DishModel>.Failure(
                new List<string> { "No se pudo crear el plato." }, 
                HttpStatusCode.InternalServerError 
            );

        }

        return Result<DishModel>.Success(result, HttpStatusCode.Created);
    }
    
    public async Task<Result<List<DishDto>>> GetAllDishes()
    {
        var result = await _dishRepository.GetAll();

        if (result == null || result.Count == 0)
        {
            return Result<List<DishDto>>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404) }, 
                HttpStatusCode.NotFound
            );
        }

        return Result<List<DishDto>>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<DishModel>> GetDishById(int id)
    {
        var result = await _dishRepository.GetByIdAsync(id);

        if (result is null)
        {
            return Result<DishModel>.Failure(
                new List<string> { HttpStatusMessages.GetMessage(404) }, 
                HttpStatusCode.NotFound
            );
        }

        return Result<DishModel>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<DishModel>> UpdateDish(DishModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
    
        if (!validationResult.IsValid)
        {
            return Result<DishModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                HttpStatusCode.BadRequest 
            );
        }

        var result = await _dishRepository.UpdateAsync(model);
    
        if (result == null) 
        {
            return Result<DishModel>.Failure(
                new List<string> { "No se pudo actualizar el plato." }, 
                HttpStatusCode.InternalServerError 
            );
        }

        return Result<DishModel>.Success(result, HttpStatusCode.OK);
    }
    
    public async Task<Result<bool>> DeleteDish(int id)
    {
        var result = await _dishRepository.DeleteAsync(id);

        if (!result)
        {
            return Result<bool>.Failure(
                new List<string> { "No se pudo eliminar el plato." }, 
                HttpStatusCode.InternalServerError 
            );
        }

        return Result<bool>.Success(result, HttpStatusCode.OK);
    }
}