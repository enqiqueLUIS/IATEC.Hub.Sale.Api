using FluentValidation;
using Sale.Domain.Models.Sale;

namespace Sale.Application.Validators.Sale;

public class DishValidation : AbstractValidator<DishModel>
{
    public DishValidation()
    {
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
            .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("El nombre solo puede contener letras, números y espacios.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.")
            .LessThanOrEqualTo(10000).WithMessage("El precio no puede ser mayor a 10,000.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción no puede estar vacía.")
            .MaximumLength(10000).WithMessage("La descripción no puede tener más de 10,000 caracteres.");
    }
}