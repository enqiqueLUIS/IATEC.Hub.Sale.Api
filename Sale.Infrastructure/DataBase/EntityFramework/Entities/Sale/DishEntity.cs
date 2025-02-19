using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

[Table("Dish")]
public class DishEntity : BaseEntity, IIdentifiable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] public int Id { get; set; }
    
    [Required]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("description", TypeName = "varchar(max)")]
    public string Description { get; set; }

    public virtual ICollection<SaleDetailEntity> SaleDetails { get; set; }
}