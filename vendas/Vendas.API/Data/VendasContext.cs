using Microsoft.EntityFrameworkCore;
using Vendas.API.Models;

namespace Vendas.API.Data
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }        
    }
}