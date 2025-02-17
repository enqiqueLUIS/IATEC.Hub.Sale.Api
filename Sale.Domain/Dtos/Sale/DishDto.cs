namespace Sale.Domain.Dtos.Sale;

public record DishDto(
    int Id,
    string Name,
    decimal Price,
    string Description
);