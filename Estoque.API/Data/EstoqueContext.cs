using Estoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Data
{
    public class EstoqueContext(DbContextOptions<EstoqueContext> options) : DbContext(options)
    {
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}