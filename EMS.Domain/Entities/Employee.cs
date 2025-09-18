using EMS.Domain.Commond;
using EMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Entities
{
    public class Employee:BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Propiedades calculadas
        public string FullName => $"{FirstName} {LastName}";
        public int YearsOfService => DateTime.UtcNow.Year - HireDate.Year;
    }
}
