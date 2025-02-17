namespace Sale.Domain.Dtos.Sale;

public record SaleDto
(
    int Id,
    DateTime SaleDate,
    decimal Total,
    int PaymentMethodId
);