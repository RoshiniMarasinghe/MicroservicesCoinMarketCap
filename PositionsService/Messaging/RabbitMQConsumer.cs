using System.Text;
using RabbitMQ.Client;
using PositionsService.Services;
using RabbitMQ.Client.Events;
using PositionsService.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace PositionsService.Messaging
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory, ILogger<RabbitMQConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "rate_updates", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, x) =>
            {
                var body = x.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var rateChangeEvent = JsonConvert.DeserializeObject<RateChangeEvent>(message);

                if (rateChangeEvent != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var positionService = scope.ServiceProvider.GetRequiredService<IPositionService>();

                    await positionService.UpdatePositionPricesAsync(rateChangeEvent.Symbol, rateChangeEvent.NewRate);
                }
            };

            _channel.BasicConsume(queue: "rate_updates", autoAck: true, consumer: consumer);
        }
    }    
}
