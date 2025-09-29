using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class restore_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "CompanyName", "ContactName", "Country", "CreatedAt", "Email", "IsDeleted", "Notes", "Phone", "PostalCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Av. Insurgentes Sur 1234", "Ciudad de México", "Tech Solutions México SA de CV", "Carlos Rivera Mendoza", "México", new DateTime(2025, 9, 29, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(592), "carlos@techsolutions.mx", false, null, "+52 555 111 2222", "03100", null },
                    { 2, "Blvd. Manuel Ávila Camacho 567", "Guadalajara", "Innovate Corp", "Ana Martínez Silva", "México", new DateTime(2025, 9, 29, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(594), "ana@innovatecorp.com", false, null, "+52 555 333 4444", "44100", null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "Notes", "Phone", "Position", "Salary", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 29, 18, 17, 50, 796, DateTimeKind.Utc).AddTicks(8679), "Tecnología", "juan.perez@empresa.com", "Juan Carlos", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Pérez López", null, "+52 555 123 4567", "Desarrollador Senior", 75000m, 1, null },
                    { 2, new DateTime(2025, 9, 29, 18, 17, 50, 796, DateTimeKind.Utc).AddTicks(8683), "Ventas", "maria.gonzalez@empresa.com", "María Elena", new DateTime(2019, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "González Ruiz", null, "+52 555 987 6543", "Gerente de Ventas", 85000m, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "IsDeleted", "Notes", "OrderDate", "OrderNumber", "RequiredDate", "ShippedDate", "ShippingAddress", "Status", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 19, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4082), 1, false, "Orden prioritaria - Cliente premium", new DateTime(2025, 9, 19, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4072), "ORD-2024-001", new DateTime(2025, 10, 4, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4076), null, null, 2, 15000.00m, null },
                    { 2, new DateTime(2025, 9, 24, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4085), 2, false, null, new DateTime(2025, 9, 24, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4084), "ORD-2024-002", new DateTime(2025, 10, 9, 18, 17, 50, 797, DateTimeKind.Utc).AddTicks(4084), null, null, 1, 8500.00m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
