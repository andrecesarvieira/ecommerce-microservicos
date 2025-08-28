using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Services
{
    public class VendaService(IPedidoRepository pedidoRepository) : IVendaService
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

        public async Task<IEnumerable<Pedido>> ObterPedidosAsync()
        {
            return await _pedidoRepository.ObterTodosAsync();
        }

        public async Task<Pedido?> ObterPedidoPorIdAsync(int id)
        {
            return await _pedidoRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarPedidoAsync(Pedido pedido)
        {
            await _pedidoRepository.AdicionarAsync(pedido);
        }

        public async Task<Pedido?> AtualizarPedidoAsync(Pedido pedido)
        {
            var existePedido = await ObterPedidoPorIdAsync(pedido.Id);
            if (existePedido == null) return null;

            return await _pedidoRepository.AtualizarAsync(pedido);
        }

        public async Task<bool> RemoverPedidoAsync(int id)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id);
            if (pedido == null) return false;

            await _pedidoRepository.RemoverAsync(pedido);
            return true;
        }
    }
}