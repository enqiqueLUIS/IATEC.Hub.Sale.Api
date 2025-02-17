using System.Net;
using FluentAssertions;
using FluentValidation;
using Moq;
using Sale.Application.Services.Sale;
using Sale.Domain.Dtos.Sale;
using Sale.Domain.Models.Sale;
using Sale.Domain.Repositories.Sale;

namespace Sale.Domain.Test;

[TestFixture]
public class DishServiceTests
{
    private Mock<IDishRepository> _dishRepositoryMock;
    private Mock<IValidator<DishModel>> _validatorMock;
    private DishService _dishService;

    [SetUp]
    public void Setup()
    {
            _dishRepositoryMock = new Mock<IDishRepository>();
            _validatorMock = new Mock<IValidator<DishModel>>();
            _dishService = new DishService(_validatorMock.Object, _dishRepositoryMock.Object);
    }

    [Test]
    public async Task CreateDish_ShouldReturnSuccess_WhenValidDish()
    {
        var dish = new DishModel(1, "Picante de Pollo", 12.99m, "Deliciosa pollo");
        _validatorMock.Setup(v => v.ValidateAsync(dish, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _dishRepositoryMock.Setup(r => r.CreateAsync(dish)).ReturnsAsync(dish);
        
        var result = await _dishService.CreateDish(dish);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Picante de Pollo");
        result.Data.Price.Should().Be(12.99m);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task GetAllDishes_ShouldReturnDishesList_WhenDataExists()
    {
        var dishes = new List<DishModel>
        {
            new(1, "Picante de Pollo", 12.99m, "Deliciosa pollo"),
            new(2, "Charque", 15.50m, "Carne seca")
        };
        
        var dishesDto = dishes.Select(d => new DishDto(d.Id, d.Name, d.Price, d.Description)).ToList();

        _dishRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(dishesDto);
        
        var result = await _dishService.GetAllDishes();
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Name.Should().Be("Picante de Pollo");
        result.Data[1].Name.Should().Be("Charque");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Test]
    public async Task GetDishById_ShouldReturnDish_WhenIdExists()
    {
        var dish = new DishModel(1, "Pique", 9.99m, "Deliciosa carne");
        _dishRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dish);
        
        var result = await _dishService.GetDishById(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Pique");
        result.Data.Price.Should().Be(9.99m);
    }

    [Test]
    public async Task GetDishById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        _dishRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((DishModel)null);
        
        var result = await _dishService.GetDishById(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se encontró el recurso solicitado.");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task UpdateDish_ShouldReturnSuccess_WhenDishIsUpdated()
    {
        var dish = new DishModel(1, "Pailita", 10.50m, "Muy buena");
        _validatorMock.Setup(v => v.ValidateAsync(dish, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _dishRepositoryMock.Setup(r => r.UpdateAsync(dish)).ReturnsAsync(dish);
        
        var result = await _dishService.UpdateDish(dish);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Pailita");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeleteDish_ShouldReturnSuccess_WhenDishIsDeleted()
    {
        _dishRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await _dishService.DeleteDish(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeleteDish_ShouldReturnFailure_WhenDishDoesNotExist()
    {
        _dishRepositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);
        
        var result = await _dishService.DeleteDish(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se pudo eliminar el plato.");
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}