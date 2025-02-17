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
public class SaleDetailServiceTests
{
    private Mock<ISaleDetailRepository> _saleDetailRepositoryMock;
    private Mock<IValidator<SaleDetailModel>> _validatorMock;
    private SaleDetailService _saleDetailService;

    [SetUp]
    public void Setup()
    {
        _saleDetailRepositoryMock = new Mock<ISaleDetailRepository>();
        _validatorMock = new Mock<IValidator<SaleDetailModel>>();
        _saleDetailService = new SaleDetailService(_validatorMock.Object, _saleDetailRepositoryMock.Object);
    }

    [Test]
    public async Task CreateSaleDetail_ShouldReturnSuccess_WhenValidSaleDetail()
    {
        var saleDetail = new SaleDetailModel(1, 101, 202, 2, 19.98m);
        _validatorMock.Setup(v => v.ValidateAsync(saleDetail, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _saleDetailRepositoryMock.Setup(r => r.CreateAsync(saleDetail)).ReturnsAsync(saleDetail);
        
        var result = await _saleDetailService.CreateSaleDetail(saleDetail);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Amount.Should().Be(2);
        result.Data.SubTotal.Should().Be(19.98m);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task GetAllSaleDetails_ShouldReturnSaleDetailsList_WhenDataExists()
    {
        var saleDetails = new List<SaleDetailModel>
        {
            new(1, 101, 202, 2, 19.98m),
            new(2, 102, 203, 1, 9.99m) 
        };
        
        var saleDetailsDto = saleDetails.Select(s => new SaleDetailDto(s.Id, s.SaleId, s.DishId, s.Amount, s.SubTotal)).ToList();

        _saleDetailRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(saleDetailsDto);
        
        var result = await _saleDetailService.GetAllSaleDetails();
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].DishId.Should().Be(202); 
        result.Data[1].DishId.Should().Be(203); 
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Test]
    public async Task GetSaleDetailById_ShouldReturnSaleDetail_WhenIdExists()
    {
        var saleDetail = new SaleDetailModel(1, 101, 202, 3, 29.97m);
        _saleDetailRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(saleDetail);
        
        var result = await _saleDetailService.GetSaleDetailById(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Amount.Should().Be(3);
        result.Data.SubTotal.Should().Be(29.97m);
    }

    [Test]
    public async Task GetSaleDetailById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        _saleDetailRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((SaleDetailModel)null);
        
        var result = await _saleDetailService.GetSaleDetailById(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se encontró el recurso solicitado.");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task UpdateSaleDetail_ShouldReturnSuccess_WhenSaleDetailIsUpdated()
    {
        var saleDetail = new SaleDetailModel(1, 101, 202, 5, 49.95m);
        _validatorMock.Setup(v => v.ValidateAsync(saleDetail, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _saleDetailRepositoryMock.Setup(r => r.UpdateAsync(saleDetail)).ReturnsAsync(saleDetail);
        
        var result = await _saleDetailService.UpdateSaleDetail(saleDetail);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Amount.Should().Be(5);
        result.Data.SubTotal.Should().Be(49.95m);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeleteSaleDetail_ShouldReturnSuccess_WhenSaleDetailIsDeleted()
    {
        _saleDetailRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await _saleDetailService.DeleteSaleDetail(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeleteSaleDetail_ShouldReturnFailure_WhenSaleDetailDoesNotExist()
    {
        _saleDetailRepositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);
        
        var result = await _saleDetailService.DeleteSaleDetail(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Detalle de venta no encontrado.");
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}