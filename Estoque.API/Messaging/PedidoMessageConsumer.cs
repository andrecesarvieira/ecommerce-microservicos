using System.Text;
using System.Text.Json;
using Estoque.API.Data;
using Estoque.API.Events;
using Estoque.API.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Estoque.API.Messaging;

public class PedidoMessageConsumer(IServiceScopeFactory scopeFactory) : IPedidoMessageConsumer
{
    private readonly string _hostname = "localhost";
    private readonly string _queuePedidosCriados = "pedidos-criados";
    private readonly string _queuePedidosCancelados = "pedidos-cancelados";
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task ConsumeMessagesAsync(CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory() { HostName = _hostname };
        var connection = await factory.CreateConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await ConsumirFilaAsync(channel, _queuePedidosCriados, ProcessarPedidoCriadoAsync, cancellationToken);
        await ConsumirFilaAsync(channel, _queuePedidosCancelados, ProcessarPedidoCanceladoAsync, cancellationToken);
    }

    private static async Task ConsumirFilaAsync(IChannel channel, string queueName, Func<string, Task> processar, CancellationToken cancellationToken)
    {
        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await processar(message);
            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
    }

    private async Task ProcessarPedidoCriadoAsync(string message)
    {
        var pedido = JsonSerializer.Deserialize<PedidoEvents>(message);
        if (pedido != null)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EstoqueContext>();
            var produto = db.Produtos.FirstOrDefault(p => p.Id == pedido.ProdutoId);
            if (produto != null)
            {
                produto.Quantidade -= pedido.Quantidade;
                await db.SaveChangesAsync();
            }
        }
    }

    private async Task ProcessarPedidoCanceladoAsync(string message)
    {
        var eventoCancelado = JsonSerializer.Deserialize<PedidoCanceladoEvent>(message);
        if (eventoCancelado != null)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EstoqueContext>();
            var produto = db.Produtos.FirstOrDefault(p => p.Id == eventoCancelado.ProdutoId);
            if (produto != null)
            {
                produto.Quantidade += eventoCancelado.Quantidade;
                await db.SaveChangesAsync();
            }
        }
    }
}
