using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

[Table("Sale")]
public class SaleEntity: BaseEntity, IIdentifiable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] public int Id { get; set; }
    
    [Required]
    [Column("saleDate", TypeName = "date")]
    public DateTime SaleDate { get; set; }

    [Required]
    [Column("total", TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [Required]
    [Column("paymentMethod")]
    public int PaymentMethodId { get; set; }

    [ForeignKey("PaymentMethodId")]
    public virtual PaymentMethodEntity PaymentMethod { get; set; }

    public virtual ICollection<SaleDetailEntity> SaleDetails { get; set; }
}