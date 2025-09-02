using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Vendas.API.Events;
using Vendas.API.Interfaces;

namespace Vendas.API.Messaging
{
    public class PedidoMessagePublisher : IPedidoMessagePublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _queuePedidosCriados = "pedidos-criados";
        private readonly string _queuePedidosCancelados = "pedidos-cancelados";

        public async Task PublicarPedido(PedidoEvents mensagem)
        {
            await PublicarMensagemAsync(_queuePedidosCriados, mensagem);
        }

        public async Task PublicarPedidoCanceladoAsync(PedidoCanceladoEvent evento)
        {
            await PublicarMensagemAsync(_queuePedidosCancelados, evento);
        }

        private async Task PublicarMensagemAsync(string queueName, object mensagem)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));
            var props = new BasicProperties();
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: CancellationToken.None
            );
        }
    }
}