using Microsoft.EntityFrameworkCore;
using Auth.API.Models;

namespace Auth.API.Data
{
    public class AuthContext(IConfiguration configuracaoApp, DbContextOptions<AuthContext> options) : DbContext(options)
    {
        private readonly IConfiguration _configuracaoApp = configuracaoApp;
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador Ecommerce",
                    Email = "admin@ecommerce.com",
                    SenhaHash = "AQAAAAIAAYagAAAAEIRUj/GvsCnvX7+Lj607zu37I7bfhpxxrZ3/UgAN/EswUzacvkBMNOkdWw47E2Ix+Q==",
                    Perfil = "Administrador"
                }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var connectionString = _configuracaoApp.GetConnectionString("DefaultConnection")?.ToString();

            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}