using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace PlatformService.DataServices;

public class MessageBusClient(IOptions<RabbitMQSettings> options) : IMessageBusClient, IAsyncDisposable
{
    private readonly RabbitMQSettings _options = options.Value;
    private IConnection? _connection;
    private IChannel? _channel;


    public async Task PublishNewPlatformAsync(PlatformPublishDto platformPublishedDto, CancellationToken ct = default)
    {
        await InitialzeProperities(ct);
        if (_channel is null || _connection is null)
        {
            throw new InvalidOperationException("Channel or Connection is not initialized.");
        }

        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            await SendMessage(message, ct);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ Connection Closed, not sending message...");
        }
    }
    
    private async Task InitialzeProperities(CancellationToken ct)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Host,
            Port = _options.Port
        };
        _connection = await factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        await _channel.ExchangeDeclareAsync(
            exchange: "trigger",
            type: ExchangeType.Fanout,
            cancellationToken: ct
            );

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

        Console.WriteLine("--> Connected to RabbitMQ");
    }

    private async Task SendMessage(string message, CancellationToken ct)
    {
        if (_channel is null)
            throw new InvalidOperationException("Channel is not initialized.");

        var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message));

        await _channel.BasicPublishAsync(
            exchange: "trigger",
            routingKey: "",
            mandatory: false,
            body: body,
            cancellationToken: ct);

        Console.WriteLine($"--> We have sent {message}");
    }
    private Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ connection shutdown: " + e.ReplyText);
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