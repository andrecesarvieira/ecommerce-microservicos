using Estoque.API.Dtos;
using Estoque.API.Interfaces;
using Estoque.API.Models;

namespace Estoque.API.Services
{
    public class EstoqueService(IProdutoRepository produtoRepository) : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<IEnumerable<Produto>> ObterProdutosAsync(int pagina)
        {
            return await _produtoRepository.ObterTodosAsync(pagina);
        }

        public async Task<Produto?> ObterProdutoPorIdAsync(int id)
        {
            return await _produtoRepository.ObterPorIdAsync(id);
        }

        public async Task<int> IncluirProdutoAsync(ProdutoDto produtoDto)
        {
            return await _produtoRepository.IncluirAsync(produtoDto);
        }

        public async Task<Produto?> AtualizarProdutoAsync(Produto produto)
        {
            return await _produtoRepository.AtualizarAsync(produto);
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return false;

            await _produtoRepository.RemoverAsync(produto);
            return true;
        }
    }
}