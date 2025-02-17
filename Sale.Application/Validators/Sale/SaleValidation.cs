using FluentValidation;
using Sale.Domain.Models.Sale;

namespace Sale.Application.Validators.Sale;

public class SaleValidation : AbstractValidator<SaleModel>
{
    public SaleValidation()
    {
        RuleFor(x => x.SaleDate.Date)
            .NotEmpty().WithMessage("La fecha de venta no puede ser vacía.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de venta no puede ser en el futuro.");

        RuleFor(x => x.Total)
            .GreaterThan(0).WithMessage("El total de la venta debe ser mayor que 0.")
            .LessThanOrEqualTo(100000).WithMessage("El total de la venta no puede superar los 100,000.");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0).WithMessage("El ID del método de pago debe ser un número positivo mayor que 0.");
    }
}