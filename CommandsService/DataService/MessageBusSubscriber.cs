
using CommandsService.IEventProcessing;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.DataService;

public class MessageBusSubscriber(
    IEventProcessor _eventProcessor,
    IOptions<RabbitMQSettings> options
    ) : BackgroundService, IAsyncDisposable
{
    private readonly RabbitMQSettings _options = options.Value;
    private IConnection _connection = default!;
    private IChannel _channel = default!;
    private QueueDeclareOk _queueName = default!;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        await InitializeRabbitMQ(stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (ModuleHandle, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"---> Event received: {message}");

            return _eventProcessor.ProcessEvent(message);
        };

        await _channel.BasicConsumeAsync(
            queue: _queueName.QueueName, 
            autoAck: true, 
            consumer: consumer, 
            cancellationToken: stoppingToken);


    }

    private async Task InitializeRabbitMQ(CancellationToken ct = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
        };

        _connection = await factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

        await _channel.ExchangeDeclareAsync(
            exchange: "trigger",
            type: ExchangeType.Fanout,
            arguments: null,
            cancellationToken: ct);

        _queueName = await _channel.QueueDeclareAsync(cancellationToken: ct);

        await _channel.QueueBindAsync(
            queue: _queueName.QueueName,
            exchange: "trigger",
            routingKey: string.Empty,
            cancellationToken: ct);

        Console.WriteLine("---> Listenting on the message bus ...");

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdownAsync;

    }
    private Task RabbitMQ_ConnectionShutdownAsync(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("---> RabbitMQ connection shutdown ...");
        return Task.CompletedTask;
    }
    public async ValueTask DisposeAsync()
    {
        if (_channel is not null && _channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
            await _channel.DisposeAsync();
        }

        if (_connection is not null && _connection.IsOpen)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
