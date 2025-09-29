using EMS.API.Commond;
using EMS.Domain.Enums;

namespace EMS.API.Models;
public class EmployeeQuery : PagedQuery
{
    public string? Department { get; set; }
    public EmployeeStatus? Status { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime? HiredAfter { get; set; }
    public DateTime? HiredBefore { get; set; }
}
