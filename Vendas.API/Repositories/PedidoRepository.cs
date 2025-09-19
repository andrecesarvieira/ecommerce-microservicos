
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Repositories;

public class PedidoRepository(VendasContext vendasContext, ILogger<PedidoRepository> logger) : IPedidoRepository
{
private readonly VendasContext _context = vendasContext;
private readonly ILogger<PedidoRepository> _logger = logger;

    public async Task<IEnumerable<Pedido>> ObterTodosAsync(int pagina)
    {
        const int itensPorPagina = 10;

        var query = _context.Pedidos;

        return await query
            .Skip((pagina - 1) * itensPorPagina)
            .Take(itensPorPagina)
            .ToListAsync();
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
            _logger.LogError(ex, "Erro ao incluir o pedido no banco de dados");
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
            _logger.LogError(ex, "Erro ao cancelar o pedido no banco de dados");
            throw new Exception("Erro ao cancelar o pedido no banco de dados.", ex);
        }
    }
}
