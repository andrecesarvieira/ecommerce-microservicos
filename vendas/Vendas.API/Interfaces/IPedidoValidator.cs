using Vendas.API.Models;

namespace Vendas.API.Interfaces
{
    public interface IPedidoValidator
    {
        Task<string?> ValidarAsync(Pedido pedido);
    }
}