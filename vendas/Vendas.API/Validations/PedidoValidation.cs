using Vendas.API.Dtos;

namespace Vendas.API.Validations
{
    public class PedidoValidation
    {
        public List<string> ValidaDto(PedidoDto pedidoDto)
        {
            List<string> validacao = [];

            if (pedidoDto.ProdutoId == 0)
            {
                validacao.Add("Id do Produto não pode ser zero");
            }
            if (pedidoDto.Quantidade == 0)
            {
                validacao.Add("Quantidade não pode ser zero");
            }
            return validacao;
        }        
    }
}