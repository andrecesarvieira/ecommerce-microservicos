using Estoque.API.Dtos;
using Estoque.API.Models;

namespace Estoque.API.Interfaces
{
    public interface IEstoqueService
    {
        Task<List<Produto>> ObterProdutosAsync(int pagina);
        Task<Produto?> ObterProdutoPorIdAsync(int id);
        Task<int> IncluirProdutoAsync(ProdutoDto dto);
        Task<Produto?> AtualizarProdutoAsync(Produto produto);
        Task<bool> RemoverProdutoAsync(int id);        
    }
}