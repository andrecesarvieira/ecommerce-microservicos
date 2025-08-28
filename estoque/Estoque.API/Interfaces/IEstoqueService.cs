using Estoque.API.Models;

namespace Estoque.API.Interfaces
{
    public interface IEstoqueService
    {
        Task<IEnumerable<Produto>> ObterProdutosAsync();
        Task<Produto?> ObterProdutoPorIdAsync(int id);
        Task IncluirProdutoAsync(Produto produto);
        Task<Produto?> AtualizarProdutoAsync(Produto produto);
        Task<bool> RemoverProdutoAsync(int id);        
    }
}