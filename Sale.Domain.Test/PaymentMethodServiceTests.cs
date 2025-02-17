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
public class PaymentMethodServiceTests
{
    private Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock;
    private Mock<IValidator<PaymentMethodModel>> _validatorMock;
    private PaymentMethodService _paymentMethodService;

    [SetUp]
    public void Setup()
    {
        _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        _validatorMock = new Mock<IValidator<PaymentMethodModel>>();
        _paymentMethodService = new PaymentMethodService(_validatorMock.Object, _paymentMethodRepositoryMock.Object);
    }

    [Test]
    public async Task CreatePaymentMethod_ShouldReturnSuccess_WhenValidPaymentMethod()
    {
        var paymentMethod = new PaymentMethodModel(1, "Credit Card");
        _validatorMock.Setup(v => v.ValidateAsync(paymentMethod, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _paymentMethodRepositoryMock.Setup(r => r.CreateAsync(paymentMethod)).ReturnsAsync(paymentMethod);
        
        var result = await _paymentMethodService.CreatePaymentMethod(paymentMethod);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Credit Card");
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task GetAllPaymentMethods_ShouldReturnPaymentMethodsList_WhenDataExists()
    {
        var paymentMethods = new List<PaymentMethodModel>
        {
            new(1, "Credit Card"),
            new(2, "PayPal")
        };
        
        var paymentMethodsDto = paymentMethods.Select(p => new PaymentMethodDto(p.Id, p.Name)).ToList();

        _paymentMethodRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(paymentMethodsDto);
        
        var result = await _paymentMethodService.GetAllPaymentMethods();
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Name.Should().Be("Credit Card");
        result.Data[1].Name.Should().Be("PayPal");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Test]
    public async Task GetPaymentMethodById_ShouldReturnPaymentMethod_WhenIdExists()
    {
        var paymentMethod = new PaymentMethodModel(1, "Efectivo");
        _paymentMethodRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(paymentMethod);
        
        var result = await _paymentMethodService.GetPaymentMethodsById(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Efectivo");
    }

    [Test]
    public async Task GetPaymentMethodById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        _paymentMethodRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((PaymentMethodModel)null);
        
        var result = await _paymentMethodService.GetPaymentMethodsById(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se encontró el recurso solicitado.");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task UpdatePaymentMethod_ShouldReturnSuccess_WhenPaymentMethodIsUpdated()
    {
        var paymentMethod = new PaymentMethodModel(1, "Transferencia bancaria");
        _validatorMock.Setup(v => v.ValidateAsync(paymentMethod, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _paymentMethodRepositoryMock.Setup(r => r.UpdateAsync(paymentMethod)).ReturnsAsync(paymentMethod);
        
        var result = await _paymentMethodService.UpdatePaymentMethod(paymentMethod);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Transferencia bancaria");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeletePaymentMethod_ShouldReturnSuccess_WhenPaymentMethodIsDeleted()
    {
        _paymentMethodRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await _paymentMethodService.DeletePaymentMethod(1);
        
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task DeletePaymentMethod_ShouldReturnFailure_WhenPaymentMethodDoesNotExist()
    {
        _paymentMethodRepositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);
        
        var result = await _paymentMethodService.DeletePaymentMethod(999);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("No se pudo eliminar el metodo de pago");
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}