
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var fact = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    Password = "guest",
    UserName = "guest"
};

using (IConnection connection = fact.CreateConnection())
{
    using (IModel channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

        string queueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue: queueName,
            exchange: "direct_logs",
            routingKey: "info");

        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());

            Console.WriteLine($"Message Recieved [RouteKey]: info, [Message]: " + message);
        };

        channel.BasicConsume(queueName, true, consumer);

        Console.WriteLine("Info consume");

        Console.ReadLine();
    }
}