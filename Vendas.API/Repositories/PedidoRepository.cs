
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Repositories
{
    public class PedidoRepository(VendasContext vendasContext, ILogger<PedidoRepository> logger) : IPedidoRepository
    {
    private readonly VendasContext _context = vendasContext;
    private readonly ILogger<PedidoRepository> _logger = logger;

        public async Task<IEnumerable<Pedido>> ObterTodosAsync()
        {
            _logger.LogInformation("Buscando todos os pedidos no banco de dados");
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Buscando pedido por id: {Id}", id);
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            try
            {
                _logger.LogInformation("Adicionando novo pedido: {@Pedido}", pedido);
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pedido adicionado com sucesso: {@Pedido}", pedido);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao incluir o pedido no banco de dados");
                throw new Exception("Erro ao incluir o pedido no banco de dados.", ex);
            }
        }

        public async Task CancelarAsync(Pedido pedido)
        {
            try
            {
                _logger.LogInformation("Cancelando pedido: {@Pedido}", pedido);
                pedido.Status = StatusPedido.Cancelado;
                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pedido cancelado com sucesso: {@Pedido}", pedido);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao cancelar o pedido no banco de dados");
                throw new Exception("Erro ao cancelar o pedido no banco de dados.", ex);
            }
        }
    }
}