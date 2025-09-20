using EMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Infrastructure.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.ContactName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.Country)
            .HasMaxLength(100);

        builder.Property(c => c.PostalCode)
            .HasMaxLength(20);

        builder.Property(c => c.Notes)
            .HasMaxLength(500);

        // Seed data
        builder.HasData(
            new Customer
            {
                Id = 1,
                CompanyName = "Tech Solutions México SA de CV",
                ContactName = "Carlos Rivera Mendoza",
                Email = "carlos@techsolutions.mx",
                Phone = "+52 555 111 2222",
                Address = "Av. Insurgentes Sur 1234",
                City = "Ciudad de México",
                Country = "México",
                PostalCode = "03100",
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Id = 2,
                CompanyName = "Innovate Corp",
                ContactName = "Ana Martínez Silva",
                Email = "ana@innovatecorp.com",
                Phone = "+52 555 333 4444",
                Address = "Blvd. Manuel Ávila Camacho 567",
                City = "Guadalajara",
                Country = "México",
                PostalCode = "44100",
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}

