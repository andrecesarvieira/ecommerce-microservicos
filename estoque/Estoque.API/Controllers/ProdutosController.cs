using Estoque.API.Dtos;
using Estoque.API.Interfaces;
using Estoque.API.Validations;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ProdutosController(IEstoqueService estoqueService) : ControllerBase
    {
        private readonly IEstoqueService _estoqueService = estoqueService;

        [HttpGet]
        public async Task<IActionResult> ObterProdutos()
        {
            var produtos = await _estoqueService.ObterProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProdutoPorId(int id)
        {
            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirProduto([FromBody] ProdutoDto produtoDto)
        {
            var validacao = new ProdutoValidation().ValidaDto(produtoDto);
            if (validacao.Count > 0) return BadRequest(validacao);

            var produtoId = await _estoqueService.IncluirProdutoAsync(produtoDto);
            return CreatedAtAction(nameof(ObterProdutoPorId), new { id = produtoId }, produtoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] ProdutoDto produtoDto)
        {
            var validacao = new ProdutoValidation().ValidaDto(produtoDto);
            if (validacao.Count > 0) return BadRequest(validacao);

            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            if (produto == null)
                return NotFound();

            produto.Nome = produtoDto.Nome;
            produto.Descricao = produtoDto.Descricao;
            produto.Quantidade = produtoDto.Quantidade;
            produto.Preco = produtoDto.Preco;

            var res = await _estoqueService.AtualizarProdutoAsync(produto);
            return res == null ? NotFound() : NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            var removido = await _estoqueService.RemoverProdutoAsync(id);
            return removido == true ? NoContent() : NotFound();
        }
    }
}