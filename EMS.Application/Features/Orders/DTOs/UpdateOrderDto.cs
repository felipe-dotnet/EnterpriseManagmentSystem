using EMS.Domain.Enums;

namespace EMS.Application.Features.Orders.DTOs;
public class UpdateOrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? RequiredDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public string? ShippingAddress { get; set; }
}
