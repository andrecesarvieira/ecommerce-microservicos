using Estoque.API.Dtos;

namespace Estoque.API.Validations;

public class ProdutoValidation
{
    public List<string> ValidaDto(ProdutoDto produtoDto)
    {
        List<string> validacao = [];

        if (string.IsNullOrEmpty(produtoDto.Nome) || produtoDto.Nome == "string")
        {
            validacao.Add("Nome não pode ser vazio.");
        }
        if (string.IsNullOrEmpty(produtoDto.Descricao) || produtoDto.Descricao == "string")
        {
            validacao.Add("Descrição não pode ser vazia.");
        }
        if (produtoDto.Preco <= 0)
        {
            validacao.Add("Preço não pode ser zero ou menor que zero.");
        }
        if (produtoDto.Quantidade <= 0)
        {
            validacao.Add("Quantidade não pode ser zero ou menor que zero.");
        }
        return validacao;
    }
}
