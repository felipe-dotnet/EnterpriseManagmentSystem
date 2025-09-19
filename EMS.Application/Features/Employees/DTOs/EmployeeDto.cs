using EMS.Domain.Enums;

namespace EMS.Application.Features.Employees.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public EmployeeStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Propiedades calculadas
        public string FullName { get; set; } = string.Empty;
        public int YearsOfService { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
