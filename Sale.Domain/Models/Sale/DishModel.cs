namespace Sale.Domain.Models.Sale;

public class DishModel
{
    public int Id { get;  set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    
    public DishModel(
        int id,
        string name,
        decimal price,
        string description
    )
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
    }
    
}