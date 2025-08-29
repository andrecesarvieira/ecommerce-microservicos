using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Repositories
{
    public class PedidoRepository(VendasContext vendasContext) : IPedidoRepository
    {
        private readonly VendasContext _context = vendasContext;

        public async Task<IEnumerable<Pedido>> ObterTodosAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao incluir o pedido no banco de dados.", ex);
            }
        }

        public async Task CancelarAsync(Pedido pedido)
        {
            try
            {
                pedido.Status = StatusPedido.Cancelado;
                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao cancelar o pedido no banco de dados.", ex);
            }
        }
    }
}