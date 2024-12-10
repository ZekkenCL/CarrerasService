using System.Text;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using CarreraService.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Threading;
using CarreraService.Services;

public class RabbitMQConsumer : BackgroundService
{
    private readonly RabbitMQConnection _connection;
    private readonly RabbitMQSettings _settings;

    public RabbitMQConsumer(RabbitMQConnection connection, IOptions<RabbitMQSettings> options)
    {
        _connection = connection;
        _settings = options.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
{
    var channel = _connection.CreateChannel();
    channel.QueueDeclare(_settings.Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Message Consumed: {message}");
    };

    channel.BasicConsume(queue: _settings.Queue, autoAck: true, consumer: consumer);

    return Task.CompletedTask;
}
}
