using Estoque.API.Interfaces;
using Estoque.API.Models;

namespace Estoque.API.Services
{
    public class EstoqueService(IProdutoRepository produtoRepository) : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<IEnumerable<Produto>> ObterProdutosAsync()
        {
            return await _produtoRepository.ObterTodosAsync();
        }

        public async Task<Produto?> ObterProdutoPorIdAsync(int id)
        {
            return await _produtoRepository.ObterPorIdAsync(id);
        }

        public async Task IncluirProdutoAsync(Produto produto)
        {
            await _produtoRepository.IncluirAsync(produto);
        }

        public async Task<Produto?> AtualizarProdutoAsync(Produto produto)
        {
            var existeProduto = await ObterProdutoPorIdAsync(produto.Id);
            if (existeProduto == null) return null;

            return await _produtoRepository.AtualizarAsync(produto);
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null) return false;

            await _produtoRepository.RemoverAsync(produto);
            return true;
        }
    }
}