using PositionsService.Messaging;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly IRabbitMQConsumer _consumer;
    private readonly ILogger<RabbitMQBackgroundService> _logger;

    public RabbitMQBackgroundService(IRabbitMQConsumer consumer, ILogger<RabbitMQBackgroundService> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.StartListening();
        return Task.CompletedTask;
    }
}
