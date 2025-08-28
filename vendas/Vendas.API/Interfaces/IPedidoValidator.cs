using Vendas.API.Dtos;
using Vendas.API.Models;

namespace Vendas.API.Interfaces
{
    public interface IPedidoValidator
    {
        Task<string?> VerificarEstoque(PedidoDto dto);
    }
}