using Vendas.API.Events;

namespace Vendas.API.Interfaces
{
    public interface IPedidoMessagePublisher
    {
        Task PublicarPedido(PedidoEvents mensagem);
        Task PublicarPedidoCanceladoAsync(PedidoCanceladoEvent evento);
    }
}