using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Models;

namespace Vendas.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly VendasContext _context;

        public PedidosController(VendasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPedidos()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedidoPorId(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPedidos), new { id = pedido.Id}, pedido);
        }
    }
}