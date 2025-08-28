using Vendas.API.Events;

namespace Vendas.API.Interfaces
{
    public interface IPedidoMessagePublisher
    {
        Task PublicarPedido(object mensagem);
        Task PublicarPedidoCanceladoAsync(PedidoCanceladoEvent evento);
    }
}