using EMS.Application.Commond.Interfaces;
using EMS.Domain.Entities;
using EMS.Domain.Enums;
using EMS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Infrastructure.Repositories;
public class OrderRepository(ApplicationDbContext context):BaseRepository<Order>(context), IOrderRepository
{
    public async Task<IReadOnlyList<Order>> GetOrdersByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSalesByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var total = await _dbSet
            .Where(o => o.OrderDate >= startDate
                     && o.OrderDate <= endDate
                     && o.Status != OrderStatus.Cancelled)
            .SumAsync(o => o.TotalAmount);

        return total;
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month;

        // Contar órdenes del mes actual
        var monthlyCount = await _dbSet
            .CountAsync(o => o.OrderDate.Year == year && o.OrderDate.Month == month);

        // Formato: ORD-YYYY-MM-###
        return $"ORD-{year:D4}-{month:D2}-{(monthlyCount + 1):D3}";
    }

    // Override para incluir lógica específica de órdenes
    public override async Task<Order> AddAsync(Order order)
    {
        // Generar número de orden si no existe
        if (string.IsNullOrEmpty(order.OrderNumber))
        {
            order.OrderNumber = await GenerateOrderNumberAsync();
        }

        // Validar que el número de orden sea único
        var orderExists = await ExistsAsync(o => o.OrderNumber == order.OrderNumber);
        if (orderExists)
        {
            // Si ya existe, generar uno nuevo
            order.OrderNumber = await GenerateOrderNumberAsync();
        }

        // Validar que el cliente existe
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == order.CustomerId);
        if (!customerExists)
            throw new InvalidOperationException($"No existe un cliente con ID: {order.CustomerId}");

        order.CreatedAt = DateTime.UtcNow;
        return await base.AddAsync(order);
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public override async Task<IReadOnlyList<Order>> GetAllAsync()
    {
        return await _dbSet
            .Include(o => o.Customer)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
}
