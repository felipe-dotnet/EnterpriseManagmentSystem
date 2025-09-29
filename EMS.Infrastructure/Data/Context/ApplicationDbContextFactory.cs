using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Data.Context
{
    /// <summary>
    /// Factory para crear el DbContext en tiempo de diseño (migraciones)
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Connection string hardcoded para migraciones (cambiar si usas otra instancia)
            optionsBuilder.UseSqlServer(
                "Server=yoda;Database=EMS;User Id=sa;Password=M1b0ch0;TrustServerCertificate=true;",
                b => b.MigrationsAssembly("EMS.Infrastructure"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
