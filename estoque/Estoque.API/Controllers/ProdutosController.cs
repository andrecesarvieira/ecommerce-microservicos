using Estoque.API.Dtos;
using Estoque.API.Interfaces;
using Estoque.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController(IEstoqueService estoqueService) : ControllerBase
    {
        private readonly IEstoqueService _estoqueService = estoqueService;

        [HttpGet("ObterProdutos")]
        public async Task<IActionResult> ObterProdutos()
        {
            var produtos = await _estoqueService.ObterProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet("ObterProdutoPorId/{id}")]
        public async Task<IActionResult> ObterProdutoPorId(int id)
        {
            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPost("IncluirProduto")]
        public async Task<IActionResult> IncluirProduto([FromBody] Produto produto)
        {
            await _estoqueService.IncluirProdutoAsync(produto);
            return CreatedAtAction(nameof(ObterProdutos), new { id = produto.Id }, produto);
        }

        [HttpPut("AtualizarProduto")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] AtualizarProdutoDto dto)
        {
            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            if (produto == null)
                return NotFound();

            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Quantidade = dto.Quantidade;
            produto.Preco = dto.Preco;

            var res = await _estoqueService.AtualizarProdutoAsync(produto);
            return res == null ? NotFound() : CreatedAtAction(nameof(ObterProdutos), new { id = produto.Id }, produto);
        }
        
        [HttpDelete("RemoverProduto/{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            var removido = await _estoqueService.RemoverProdutoAsync(id);
            return removido == true ? NoContent() : NotFound();
        }
    }
}