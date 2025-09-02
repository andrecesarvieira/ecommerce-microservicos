using Microsoft.AspNetCore.Mvc;
using Vendas.API.Models;
using Vendas.API.Interfaces;
using Vendas.API.Dtos;
using Vendas.API.Events;
using Vendas.API.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vendas.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Vendas")]
    [Route("api/[controller]")]
    public class PedidosController(
        IVendaService vendaService,
        IPedidoMessagePublisher publisher,
        IPedidoValidator pedidoValidator) : ControllerBase
    {
        private readonly IVendaService _vendaService = vendaService;
        private readonly IPedidoMessagePublisher _publisher = publisher;
        private readonly IPedidoValidator _pedidoValidator = pedidoValidator;
        private const string msgPedidoNaoEncontrado = "Pedido(s) não encontrado(s).";

        // Endpoint obter lista de pedidos
        [HttpGet]
        public async Task<IActionResult> ObterPedidos([FromQuery, BindRequired] int pagina)
        {
            var pedidosList = await _vendaService.ObterPedidosAsync(pagina);
            var pedidos = pedidosList.ToList();
            return pedidos == null ? NotFound(msgPedidoNaoEncontrado) : Ok(pedidos);
        }

        // Endpoint obter pedido por id
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            return pedido == null ? NotFound(msgPedidoNaoEncontrado) : Ok(pedido);
        }

        // Endpoint incluir novo pedido
        [HttpPost]
        public async Task<IActionResult> IncluirPedido([FromBody] PedidoDto pedidoDto)
        {
            var validacao = new PedidoValidation().ValidaDto(pedidoDto);
            if (validacao.Count > 0)
                return BadRequest(validacao);

            var erro = await _pedidoValidator.VerificarEstoque(pedidoDto);
            if (erro != null)
                return BadRequest(erro);

            Pedido pedido = new()
            {
                ProdutoId = pedidoDto.ProdutoId,
                Quantidade = pedidoDto.Quantidade
            };

            await _vendaService.AdicionarPedidoAsync(pedido);
            await _publisher.PublicarPedido(new PedidoEvents { ProdutoId = pedido.ProdutoId, Quantidade = pedido.Quantidade });
            return CreatedAtAction(nameof(ObterPedidos), new { id = pedido.Id }, pedido);
        }

        // Endpoint cancelar pedido (exclusão lógica)
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null)
                return NotFound(msgPedidoNaoEncontrado);

            var jaCancelado = new PedidoValidation().JaCancelado(pedido);
            if (jaCancelado != null)
                return BadRequest(jaCancelado);

            await _vendaService.CancelarPedidoAsync(pedido);

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