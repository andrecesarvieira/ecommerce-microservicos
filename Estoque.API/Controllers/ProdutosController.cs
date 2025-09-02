using Estoque.API.Dtos;
using Estoque.API.Interfaces;
using Estoque.API.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ProdutosController(IEstoqueService estoqueService) : ControllerBase
    {
        private readonly IEstoqueService _estoqueService = estoqueService;
        private const string msgProdutoNaoEncontrado = "Produto(s) não encontrados(s).";

        // Endpoint obter lista de produtos
        [HttpGet]
        [Authorize(Roles = "Estoque")]
        public async Task<IActionResult> ObterProdutos([FromQuery, BindRequired] int pagina)
        {
            var produtosList = await _estoqueService.ObterProdutosAsync(pagina);
            var produtos = produtosList.ToList();
            return produtos == null ? NotFound(msgProdutoNaoEncontrado) : Ok(produtos);
        }

        // Endpoint obter produto por id
        [HttpGet("{id}")]
        [Authorize(Roles = "Estoque,Vendas")]
        public async Task<IActionResult> ObterProdutoPorId(int id)
        {
            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            return produto == null ? NotFound(msgProdutoNaoEncontrado) : Ok(produto);
        }

        // Endpoint incluir novo produto
        [HttpPost]
        [Authorize(Roles = "Estoque")]
        public async Task<IActionResult> IncluirProduto([FromBody] ProdutoDto produtoDto)
        {
            var validacao = new ProdutoValidation().ValidaDto(produtoDto);
            if (validacao.Count > 0)
                return BadRequest(validacao);

            var produtoId = await _estoqueService.IncluirProdutoAsync(produtoDto);
            return CreatedAtAction(nameof(ObterProdutoPorId), new { id = produtoId }, produtoDto);
        }

        // Endpoint atualizar produto
        [HttpPut("{id}")]
        [Authorize(Roles = "Estoque")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] ProdutoDto produtoDto)
        {
            var validacao = new ProdutoValidation().ValidaDto(produtoDto);
            if (validacao.Count > 0)
                return BadRequest(validacao);

            var produto = await _estoqueService.ObterProdutoPorIdAsync(id);
            if (produto == null)
                return NotFound(msgProdutoNaoEncontrado);

            produto.Nome = produtoDto.Nome;
            produto.Descricao = produtoDto.Descricao;
            produto.Quantidade = produtoDto.Quantidade;
            produto.Preco = produtoDto.Preco;

            await _estoqueService.AtualizarProdutoAsync(produto);
            return NoContent();
        }

        // Endpoint excluir produto
        [HttpDelete("{id}")]
        [Authorize(Roles = "Estoque")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            var removido = await _estoqueService.RemoverProdutoAsync(id);
            return removido == true ? NoContent() : NotFound(msgProdutoNaoEncontrado);
        }
    }
}