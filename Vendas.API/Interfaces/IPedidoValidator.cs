using Vendas.API.Dtos;

namespace Vendas.API.Interfaces
{
    public interface IPedidoValidator
    {
        Task<string?> VerificarEstoque(PedidoDto dto);
    }
}