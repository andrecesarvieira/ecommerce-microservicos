using Estoque.API.Dtos;
using Estoque.API.Models;

namespace Estoque.API.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task<int> IncluirAsync(ProdutoDto produtoDto);
        Task <Produto?> AtualizarAsync(Produto produto);
        Task RemoverAsync(Produto produto);
    }
}