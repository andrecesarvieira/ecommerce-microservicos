using Estoque.API.Data;
using Estoque.API.Interfaces;
using Estoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly EstoqueContext _context;

        public ProdutoRepository(EstoqueContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task IncluirAsync(Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao incluir o produto no banco de dados.", ex);
            }
        }

        public async Task<Produto?> AtualizarAsync(Produto produto)
        {
            try
            {
                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();
                return produto;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao atualizar o produto no banco de dados.", ex);
            }
        }

        public async Task RemoverAsync(Produto produto)
        {
            try
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao excluir o produto no banco de dados.", ex);
            }
        }
    }
}