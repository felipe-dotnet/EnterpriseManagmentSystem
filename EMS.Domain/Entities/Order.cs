using EMS.Domain.Commond;
using EMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Domain.Entities;
public class Order:BaseEntity
{
    [Required]
    [StringLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.Date)]
    public DateTime? RequiredDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ShippedDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [StringLength(200)]
    public string? ShippingAddress { get; set; }

    // Propiedades calculadas
    public int DaysToShip => RequiredDate.HasValue ?
        (RequiredDate.Value - OrderDate).Days : 0;
}