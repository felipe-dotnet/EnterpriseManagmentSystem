using EMS.Application.Features.Orders.DTOs;
using EMS.Domain.Enums;

namespace EMS.Application.Services;
public interface IOrderService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(CreateOrderDto createDto);
    Task<OrderDto> UpdateAsync(UpdateOrderDto updateDto);
    Task DeleteAsync(int id);
    Task<List<OrderDto>> GetByCustomerAsync(int customerId);
    Task<List<OrderDto>> GetByStatusAsync(OrderStatus status);
    Task<decimal> GetTotalSalesByPeriodAsync(DateTime startDate, DateTime endDate);
}
