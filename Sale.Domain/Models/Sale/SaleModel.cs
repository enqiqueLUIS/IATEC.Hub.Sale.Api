namespace Sale.Domain.Models.Sale;

public class SaleModel
{
    public int Id { get; set; }
    public DateTime SaleDate { get; private set; }
    public decimal Total { get; private set; }
    public int PaymentMethodId { get; private set; }
    
    public SaleModel(
        int id,
        DateTime saleDate,
        Decimal total,
        int paymentMethodId
    )
    {
        Id = id;
        SaleDate = saleDate;
        Total = total;
        PaymentMethodId = paymentMethodId;
    }
}