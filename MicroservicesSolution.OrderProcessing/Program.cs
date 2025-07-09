// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello!");

var factory = new RabbitMQ.Client.ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "mypass",
    VirtualHost = "/"
};

using var conn = await factory.CreateConnectionAsync();
using var channel = await conn.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "bookings",
                            durable: true,
                            exclusive: false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var jsonString = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {jsonString}");
    
    // Simulate processing
    await Task.Delay(1000);
    
    Console.WriteLine("Message processed successfully.");
};

await channel.BasicConsumeAsync("bookings", true, consumer);

Console.ReadKey();

