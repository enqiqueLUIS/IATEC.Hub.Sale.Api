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
public class SaleServiceTests
{
    private Mock<ISaleRepository> _saleRepositoryMock;
    private Mock<IValidator<SaleModel>> _validatorMock;
    private SaleService _saleService;

    [SetUp]
    public void Setup()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _validatorMock = new Mock<IValidator<SaleModel>>();
        _saleService = new SaleService(_validatorMock.Object, _saleRepositoryMock.Object);
    }

    [Test]
    public async Task CreateSale_ShouldReturnSuccess_WhenValidSale()
    {
        // parte de preparación 
        var sale = new SaleModel(1, DateTime.Now, 100.50m, 2);
        _validatorMock.Setup(v => v.ValidateAsync(sale, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _saleRepositoryMock.Setup(r => r.CreateAsync(sale)).ReturnsAsync(sale);

        // parte de acción
        var result = await _saleService.CreateSale(sale);

        // y verificasion 
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Total.Should().Be(100.50m);
        result.Data.PaymentMethodId.Should().Be(2);
    }

    [Test]
    public async Task GetAllSales_ShouldReturnSalesList_WhenDataExists()
    {
        var sales = new List<SaleModel>
        {
            new(1, DateTime.Now, 150.75m, 1),
            new(2, DateTime.Now, 200.00m, 2)
        };

        var salesDto = sales.Select(s => new SaleDto(s.Id, s.SaleDate, s.Total, s.PaymentMethodId)).ToList();

        _saleRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(salesDto);
        
        var result = await _saleService.GetAllSales();
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Total.Should().Be(150.75m);
        result.Data[1].PaymentMethodId.Should().Be(2);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Test]
    public async Task GetSaleById_ShouldReturnSale_WhenIdExists()
    {
        var sale = new SaleModel(1, DateTime.Now, 99.99m, 3);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sale);
        
        var result = await _saleService.GetSaleById(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Total.Should().Be(99.99m);
        result.Data.PaymentMethodId.Should().Be(3);
    }

    [Test]
    public async Task GetSaleById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((SaleModel)null);
        
        var result = await _saleService.GetSaleById(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se encontró el recurso solicitado."); 
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Test]
    public async Task DeleteSale_ShouldReturnSuccess_WhenSaleIsDeleted()
    {
        _saleRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await _saleService.DeleteSale(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeleteSale_ShouldReturnFailure_WhenSaleDoesNotExist()
    {
        _saleRepositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);
        
        var result = await _saleService.DeleteSale(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se pudo eliminar la venta.");
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}