using Estoque.API.Data;
using Estoque.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly EstoqueContext _context;
        public ProdutosController(EstoqueContext context)
        {
            _context = context;
        }
        // public static readonly List<dynamic> _produtos = new()
        // {
        //     new { Id = 1, Nome = "Mouse Gamer", Quantidade = 10, Preco = 150.0 },
        //     new { Id = 2, Nome = "Teclado Mecânico", Quantidade = 5, Preco = 350.0 },
        //     new { Id = 3, Nome = "Monitor 27\"", Quantidade = 3, Preco = 1250.0 }
        // };

        [HttpGet]
        public async Task<IActionResult> ObterProdutos()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProdutoPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterProdutos), new { id = produto.Id}, produto);
        }
    }
}