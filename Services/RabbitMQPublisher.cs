using RabbitMQ.Client;
using System.Text;
using CarreraService.Config;
using Microsoft.Extensions.Options;
using System;
using CarreraService.Services;

public interface IMessagePublisher
{
    void Publish(string message);
}

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly RabbitMQConnection _connection;
    private readonly RabbitMQSettings _settings;

    public RabbitMQPublisher(RabbitMQConnection connection, IOptions<RabbitMQSettings> options)
    {
        _connection = connection;
        _settings = options.Value;
    }

    public void Publish(string message)
    {
        using var channel = _connection.CreateChannel();
        channel.QueueDeclare(_settings.Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: _settings.Exchange, routingKey: _settings.RoutingKey, basicProperties: null, body: body);

        Console.WriteLine($"Message Published: {message}");
    }
}
