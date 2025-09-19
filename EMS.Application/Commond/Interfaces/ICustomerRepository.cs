using EMS.Domain.Entities;

namespace EMS.Application.Commond.Interfaces;
public interface ICustomerRepository:IRepository<Customer>
{
    Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();
    Task<Customer?> GetByEmailAsync(string email);
    Task<IReadOnlyList<Customer>> SearchAsync(string searchTerm);
}
