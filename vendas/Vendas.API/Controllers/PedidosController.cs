using Microsoft.AspNetCore.Mvc;
using Vendas.API.Models;
using Vendas.API.Interfaces;
using Vendas.API.Dtos;
using Vendas.API.Events;
using Vendas.API.Validations;

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

        [HttpGet]
        public async Task<IActionResult> ObterPedidos()
        {
            var pedidos = await _vendaService.ObterPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirPedido([FromBody] PedidoDto pedidoDto)
        {
            var validacao = new PedidoValidation().ValidaDto(pedidoDto);
            if (validacao.Count > 0) return BadRequest(validacao);

            // Verifica se tem o produto e quantidade no estoque
            var erro = await _pedidoValidator.VerificarEstoque(pedidoDto);
            if (erro != null) return BadRequest(erro);

            Pedido pedido = new()
            {
                ProdutoId = pedidoDto.ProdutoId,
                Quantidade = pedidoDto.Quantidade
            };

            await _vendaService.AdicionarPedidoAsync(pedido);
            await _publisher.PublicarPedido(new { pedido.ProdutoId, pedido.Quantidade });
            return CreatedAtAction(nameof(ObterPedidos), new { id = pedido.Id }, pedido);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null) return NotFound();

            var JaCancelado = new PedidoValidation().JaCancelado(pedido);
            if (JaCancelado != null) return BadRequest(JaCancelado);

            await _vendaService.CancelarPedidoAsync(pedido);

            // Publicar evento de cancelamento
            var eventoCancelado = new PedidoCanceladoEvent
            {
                PedidoId = pedido.Id,
                ProdutoId = pedido.ProdutoId,
                Quantidade = pedido.Quantidade,
            };
            await _publisher.PublicarPedidoCanceladoAsync(eventoCancelado);
            return NoContent();
        }
    }
}