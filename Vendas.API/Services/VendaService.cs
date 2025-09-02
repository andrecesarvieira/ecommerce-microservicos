using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Services
{
    public class VendaService(IPedidoRepository pedidoRepository) : IVendaService
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

        public async Task<IEnumerable<Pedido>> ObterPedidosAsync(int pagina)
        {
            return await _pedidoRepository.ObterTodosAsync(pagina);
        }

        public async Task<Pedido?> ObterPedidoPorIdAsync(int id)
        {
            return await _pedidoRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarPedidoAsync(Pedido pedido)
        {
            await _pedidoRepository.AdicionarAsync(pedido);
        }

        public async Task CancelarPedidoAsync(Pedido pedido)
        {
            await _pedidoRepository.CancelarAsync(pedido);
        }
    }
}