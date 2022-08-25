using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Thread.Sleep(2000);


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

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            Console.WriteLine("Message recieved:" + message);
        };

        channel.BasicConsume("dev-queue", true, consumer);
    }
}
