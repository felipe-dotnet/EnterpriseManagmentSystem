using EMS.Application.Commond.Interfaces;
using EMS.Domain.Entities;
using EMS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Infrastructure.Repositories;
public class CustomerRepository(ApplicationDbContext contex) : BaseRepository<Customer>(contex), ICustomerRepository
{
    public async Task<IReadOnlyList<Customer>> GetActiveCustomersAsync()
    {
        return await _dbSet
            .Include(c => c.Orders) // Incluir órdenes para calcular estadísticas
            .OrderBy(c => c.CompanyName)
            .ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActiveCustomersAsync();

        var term = searchTerm.ToLower().Trim();

        return await _dbSet
            .Include(c => c.Orders)
            .Where(c =>
                c.CompanyName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                c.ContactName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                c.Email.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                (c.City != null && c.City.Contains(term, StringComparison.CurrentCultureIgnoreCase)) ||
                (c.Country != null && c.Country.Contains(term, StringComparison.CurrentCultureIgnoreCase)))
            .OrderBy(c => c.CompanyName)
            .ToListAsync();
    }

    // Override para incluir validaciones específicas
    public override async Task<Customer> AddAsync(Customer customer)
    {
        // Validar email único
        var emailExists = await ExistsAsync(c => c.Email.Equals(customer.Email, StringComparison.CurrentCultureIgnoreCase));
        if (emailExists)
            throw new InvalidOperationException($"Ya existe un cliente con el email: {customer.Email}");

        customer.CreatedAt = DateTime.UtcNow;
        return await base.AddAsync(customer);
    }

    public override async Task UpdateAsync(Customer customer)
    {
        // Validar email único (excluyendo el cliente actual)
        var emailExists = await _dbSet
            .AnyAsync(c => c.Email.Equals(customer.Email, StringComparison.CurrentCultureIgnoreCase) && c.Id != customer.Id);

        if (emailExists)
            throw new InvalidOperationException($"Ya existe otro cliente con el email: {customer.Email}");

        await base.UpdateAsync(customer);
    }

    // Override para soft delete con verificación de órdenes
    public override async Task DeleteAsync(Customer customer)
    {
        // Verificar si tiene órdenes activas
        var hasActiveOrders = await _context.Orders
            .AnyAsync(o => o.CustomerId == customer.Id && !o.IsDeleted);

        if (hasActiveOrders)
            throw new InvalidOperationException("No se puede eliminar un cliente con órdenes activas. Cancele las órdenes primero.");

        await base.DeleteAsync(customer);
    }
}
