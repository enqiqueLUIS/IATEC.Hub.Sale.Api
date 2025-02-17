namespace Sale.Domain.Models.Sale;

public class SaleDetailModel
{
    public int Id { get; set; }
    public int SaleId { get; private set; }
    public int DishId { get; private set; }
    public int Amount { get; private set; }
    public decimal SubTotal { get; private set; }
    
    public SaleDetailModel(
        int id,
        int saleId,
        int dishId,
        int amount,
        decimal subTotal
    )
    {
        Id = id;
        SaleId = saleId;
        DishId = dishId;
        Amount = amount;
        SubTotal = subTotal;
    }
}