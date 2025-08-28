namespace Estoque.API.Interfaces
{
    public interface IPedidoMessageConsumer
    {
        Task ConsumeMessagesAsync(CancellationToken cancellationToken = default);
    }
}