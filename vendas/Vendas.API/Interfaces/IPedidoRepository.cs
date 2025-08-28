using Vendas.API.Models;

namespace Vendas.API.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ObterTodosAsync();
        Task<Pedido?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Pedido pedido);
        Task <Pedido?> AtualizarAsync(Pedido pedido);
        Task RemoverAsync(Pedido pedido);        
    }
}