using Microsoft.AspNetCore.Mvc;
using Vendas.API.Models;
using Vendas.API.Interfaces;

namespace Vendas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController(
        IVendaService vendaService,
        IPedidoMessagePublisher publisher,
        IPedidoValidator pedidoValidator) : ControllerBase
    {
        private readonly IVendaService _vendaService = vendaService;
        private readonly IPedidoMessagePublisher _publisher = publisher;
        private readonly IPedidoValidator _pedidoValidator = pedidoValidator;

        [HttpGet("ObterPedidos")]
        public async Task<IActionResult> ObterPedidos()
        {
            var pedidos = await _vendaService.ObterPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("ObterPedidoPorId/{id}")]
        public async Task<IActionResult> ObterPedidoPorId(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost("IncluirPedido")]
        public async Task<IActionResult> IncluirPedido([FromBody] Pedido pedido)
        {
            // Validar o pedido no Estoque.API
            var erro = await _pedidoValidator.ValidarAsync(pedido);
            if (erro != null) return BadRequest(erro);

            await _vendaService.AdicionarPedidoAsync(pedido);
            await _publisher.PublicarPedido(new { pedido.ProdutoId, pedido.Quantidade });
            return CreatedAtAction(nameof(ObterPedidos), new { id = pedido.Id }, pedido);
        }
        
        [HttpDelete("RemoverPedido/{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null)
                return NotFound();

            var removido = await _vendaService.RemoverPedidoAsync(id);
            if (!removido) return NotFound();

            // Publicar evento de cancelamento
            var eventoCancelado = new Events.PedidoCanceladoEvent
            {
                PedidoId = pedido.Id,
                ProdutoId = pedido.ProdutoId,
                Quantidade = pedido.Quantidade
            };
            await _publisher.PublicarPedidoCanceladoAsync(eventoCancelado);
            return NoContent();
        }
    }
}