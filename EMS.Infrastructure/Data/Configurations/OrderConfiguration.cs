using EMS.Domain.Entities;
using EMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Infrastructure.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();

        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.Status)
            .HasConversion<int>();

        builder.Property(o => o.Notes)
            .HasMaxLength(500);

        builder.Property(o => o.ShippingAddress)
            .HasMaxLength(500);

        // Relación con Customer
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ignorar propiedades calculadas
        builder.Ignore(o => o.DaysToShip);

        // Seed data
        builder.HasData(
            new Order
            {
                Id = 1,
                OrderNumber = "ORD-2024-001",
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-10),
                RequiredDate = DateTime.UtcNow.AddDays(5),
                Status = OrderStatus.Processing,
                TotalAmount = 15000.00m,
                Notes = "Orden prioritaria - Cliente premium",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Order
            {
                Id = 2,
                OrderNumber = "ORD-2024-002",
                CustomerId = 2,
                OrderDate = DateTime.UtcNow.AddDays(-5),
                RequiredDate = DateTime.UtcNow.AddDays(10),
                Status = OrderStatus.Pending,
                TotalAmount = 8500.00m,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        );
    }
}
