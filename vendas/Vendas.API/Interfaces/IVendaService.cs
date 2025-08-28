using Vendas.API.Models;

namespace Vendas.API.Interfaces
{
    public interface IVendaService
    {
        Task<IEnumerable<Pedido>> ObterPedidosAsync();
        Task<Pedido?> ObterPedidoPorIdAsync(int id);
        Task AdicionarPedidoAsync(Pedido pedido);
        Task CancelarPedidoAsync(Pedido pedido);        
    }
}