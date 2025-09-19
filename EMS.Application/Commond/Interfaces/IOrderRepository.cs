using EMS.Domain.Entities;
using EMS.Domain.Enums;

namespace EMS.Application.Commond.Interfaces;

public interface IOrderRepository:IRepository<Order>
{
    Task<IReadOnlyList<Order>> GetOrdersByCustomerAsync(int customerId);
    Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<IReadOnlyList<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalSalesByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<string> GenerateOrderNumberAsync();
}
