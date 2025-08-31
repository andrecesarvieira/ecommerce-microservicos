using Microsoft.AspNetCore.Mvc;
using Vendas.API.Models;
using Vendas.API.Interfaces;
using Vendas.API.Dtos;
using Vendas.API.Events;
using Vendas.API.Validations;

using Microsoft.AspNetCore.Authorization;
namespace Vendas.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Vendas")]
    [Route("api/[controller]")]
    public class PedidosController(
        IVendaService vendaService,
        IPedidoMessagePublisher publisher,
        IPedidoValidator pedidoValidator,
        ILogger<PedidosController> logger) : ControllerBase
    {
        private readonly IVendaService _vendaService = vendaService;
        private readonly IPedidoMessagePublisher _publisher = publisher;
        private readonly IPedidoValidator _pedidoValidator = pedidoValidator;
        private readonly ILogger<PedidosController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> ObterPedidos()
        {
            _logger.LogInformation("Recebida requisição para listar pedidos");
            var pedidos = await _vendaService.ObterPedidosAsync();
            _logger.LogInformation("{Count} pedidos retornados", pedidos?.Count() ?? 0);
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(int id)
        {
            _logger.LogInformation("Recebida requisição para obter pedido por id: {Id}", id);
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null)
            {
                _logger.LogWarning("Pedido id {Id} não encontrado", id);
                return NotFound();
            }
            _logger.LogInformation("Pedido id {Id} retornado", id);
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirPedido([FromBody] PedidoDto pedidoDto)
        {
            _logger.LogInformation("Recebida requisição para incluir pedido: {@PedidoDto}", pedidoDto);
            var validacao = new PedidoValidation().ValidaDto(pedidoDto);
            if (validacao.Count > 0)
            {
                _logger.LogWarning("Validação falhou ao incluir pedido: {Erros}", string.Join(",", validacao));
                return BadRequest(validacao);
            }

            var erro = await _pedidoValidator.VerificarEstoque(pedidoDto);
            if (erro != null)
            {
                _logger.LogWarning("Erro ao verificar estoque: {Erro}", erro);
                return BadRequest(erro);
            }

            Pedido pedido = new()
            {
                ProdutoId = pedidoDto.ProdutoId,
                Quantidade = pedidoDto.Quantidade
            };

            await _vendaService.AdicionarPedidoAsync(pedido);
            await _publisher.PublicarPedido(new { pedido.ProdutoId, pedido.Quantidade });
            _logger.LogInformation("Pedido incluído com sucesso: {@Pedido}", pedido);
            return CreatedAtAction(nameof(ObterPedidos), new { id = pedido.Id }, pedido);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            _logger.LogInformation("Recebida requisição para cancelar pedido id: {Id}", id);
            var pedido = await _vendaService.ObterPedidoPorIdAsync(id);
            if (pedido == null)
            {
                _logger.LogWarning("Pedido id {Id} não encontrado para cancelamento", id);
                return NotFound();
            }

            var JaCancelado = new PedidoValidation().JaCancelado(pedido);
            if (JaCancelado != null)
            {
                _logger.LogWarning("Pedido id {Id} já cancelado", id);
                return BadRequest(JaCancelado);
            }

            await _vendaService.CancelarPedidoAsync(pedido);

            var eventoCancelado = new PedidoCanceladoEvent
            {
                PedidoId = pedido.Id,
                ProdutoId = pedido.ProdutoId,
                Quantidade = pedido.Quantidade,
            };
            await _publisher.PublicarPedidoCanceladoAsync(eventoCancelado);
            _logger.LogInformation("Pedido id {Id} cancelado com sucesso", id);
            return NoContent();
        }
    }
}