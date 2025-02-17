namespace Sale.Domain.Models.Sale;

public class PaymentMethodModel
{
    public int Id { get; set; }
    public string Name { get; private set; }
    
    public PaymentMethodModel(
        int id,
        string name
    )
    {
        Id = id;
        Name = name;
    }
}