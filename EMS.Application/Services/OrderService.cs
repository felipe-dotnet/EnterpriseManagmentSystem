using AutoMapper;
using EMS.Application.Commond.Interfaces;
using EMS.Application.Features.Orders.DTOs;
using EMS.Domain.Entities;
using EMS.Domain.Enums;

namespace EMS.Application.Services;
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<OrderDto> CreateAsync(CreateOrderDto createDto)
    {
        var order = _mapper.Map<Order>(createDto);
        var createdOrder = await _repository.AddAsync(order);
        return _mapper.Map<OrderDto>(createdOrder);
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order != null)
        {
            order.IsDeleted = true;
            await _repository.UpdateAsync(order);
        }
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<List<OrderDto>> GetByCustomerAsync(int customerId)
    {
        var customer = await _repository.GetOrdersByCustomerAsync(customerId);
        return _mapper.Map<List<OrderDto>>(customer);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<List<OrderDto>> GetByStatusAsync(OrderStatus status)
    {
        var orders = await _repository.GetOrdersByStatusAsync(status);
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<decimal> GetTotalSalesByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var totalSales = await _repository.GetTotalSalesByPeriodAsync(startDate, endDate);
        return totalSales;
    }

    public async Task<OrderDto> UpdateAsync(UpdateOrderDto updateDto)
    {
        var order = _mapper.Map<Order>(updateDto);
        await _repository.UpdateAsync(order);
        return _mapper.Map<OrderDto>(order);
    }
}
