namespace Sale.Domain.Dtos.Sale;

public record SaleDetailDto
(
    int Id,
    int DishId,
    int SaleId,
    int Amount,
    decimal SubTotal
);