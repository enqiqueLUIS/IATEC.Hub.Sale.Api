using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

[Table("SaleDetail")]
public class SaleDetailEntity : BaseEntity, IIdentifiable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] public int Id { get; set; }
    
    [Required]
    [Column("saleId")]
    public int SaleId { get; set; }

    [Required]
    [Column("dishId")]
    public int DishId { get; set; }

    [Required]
    [Column("amount")]
    public int Amount { get; set; }

    [Required]
    [Column("subTotal", TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }

    [ForeignKey("SaleId")]
    public virtual SaleEntity Sale { get; set; }

    [ForeignKey("DishId")]
    public virtual DishEntity Dish { get; set; }
}