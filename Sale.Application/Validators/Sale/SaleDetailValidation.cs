using FluentValidation;
using Sale.Domain.Models.Sale;

namespace Sale.Application.Validators.Sale;

public class SaleDetailValidation : AbstractValidator<SaleDetailModel>
{
    public SaleDetailValidation()
    {
        RuleFor(x => x.SaleId)
            .GreaterThan(0).WithMessage("El ID de la venta debe ser un número positivo mayor que 0.");

        RuleFor(x => x.DishId)
            .GreaterThan(0).WithMessage("El ID del platillo debe ser un número positivo mayor que 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0.")
            .LessThanOrEqualTo(100).WithMessage("No puedes vender más de 100 unidades en una sola compra.");

        RuleFor(x => x.SubTotal)
            .GreaterThan(0).WithMessage("El subtotal debe ser mayor que 0.")
            .LessThanOrEqualTo(10000).WithMessage("El subtotal no puede superar los 10,000.");
    }
}