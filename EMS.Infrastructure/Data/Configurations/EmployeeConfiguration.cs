using EMS.Domain.Entities;
using EMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Infrastructure.Data.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Department)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Salary)
            .HasPrecision(18, 2);

        builder.Property(e => e.Status)
            .HasConversion<int>();

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        // Ignorar propiedades calculadas
        builder.Ignore(e => e.FullName);
        builder.Ignore(e => e.YearsOfService);

        // Seed data
        builder.HasData(
            new Employee
            {
                Id = 1,
                FirstName = "Juan Carlos",
                LastName = "Pérez López",
                Email = "juan.perez@empresa.com",
                Phone = "+52 555 123 4567",
                Position = "Desarrollador Senior",
                Department = "Tecnología",
                HireDate = new DateTime(2020, 3, 15),
                Salary = 75000,
                Status = EmployeeStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Employee
            {
                Id = 2,
                FirstName = "María Elena",
                LastName = "González Ruiz",
                Email = "maria.gonzalez@empresa.com",
                Phone = "+52 555 987 6543",
                Position = "Gerente de Ventas",
                Department = "Ventas",
                HireDate = new DateTime(2019, 1, 10),
                Salary = 85000,
                Status = EmployeeStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}

