using EMS.Domain.Commond;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Entities;

public class Customer:BaseEntity
{
    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    // Navegación
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
