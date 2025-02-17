using FluentValidation;
using Sale.Domain.Models.Sale;

namespace Sale.Application.Validators.Sale;

public class PaymentMethodValidation : AbstractValidator<PaymentMethodModel>
{
    public PaymentMethodValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
            .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("El nombre solo puede contener letras, números y espacios.");
    }
}