using Vendas.API.Dtos;
using Vendas.API.Models;

namespace Vendas.API.Validations
{
    public class PedidoValidation
    {
        public List<string> ValidaDto(PedidoDto pedidoDto)
        {
            List<string> validacao = [];

            if (pedidoDto.ProdutoId <= 0)
            {
                validacao.Add("Id do Produto não pode ser zero.");
            }
            if (pedidoDto.Quantidade <= 0)
            {
                validacao.Add("Quantidade não pode ser zero.");
            }
            return validacao;
        }
        public string? JaCancelado(Pedido pedido)
        {
            if (pedido.Status == StatusPedido.Cancelado) return "Pedido já está cancelado.";
            return null;
        }
    }
}