using System.Threading.Tasks;
using RabbitMQ.Client;
namespace Micro.API.Services;

public class MessageProducer : IMessageProducer
{
    public async Task SendingMessage<T>(T message)
    {
        var factory = new RabbitMQ.Client.ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "mypass",
            VirtualHost = "/"
        };

        using var conn = await factory.CreateConnectionAsync();
        using var channel =await conn.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "bookings",
                                    durable: true,
                                    exclusive: false);
        var jsonString = System.Text.Json.JsonSerializer.Serialize(message);
        var body = System.Text.Encoding.UTF8.GetBytes(jsonString);

        await channel.BasicPublishAsync("", "bookings", body: body);
    }
}