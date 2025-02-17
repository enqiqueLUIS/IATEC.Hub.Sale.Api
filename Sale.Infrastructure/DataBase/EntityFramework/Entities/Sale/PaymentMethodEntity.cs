using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sale.Infrastructure.DataBase.EntityFramework.Entities.Sale;

[Table("PaymentMethod")]
public class PaymentMethodEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] public int Id { get; set; }
    
    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; }

    public virtual ICollection<SaleEntity> Sales { get; set; }
}