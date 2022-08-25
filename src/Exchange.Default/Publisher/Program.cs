using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare("dev-queue",
            false, false, false, null);

        string message = "Message from Publisher";

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("",
            "dev-queue",
            null,
            body);

        Console.WriteLine("Message sent with default exchange");
    }
}

