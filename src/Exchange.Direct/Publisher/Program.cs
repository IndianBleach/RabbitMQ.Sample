using RabbitMQ.Client;
using System.Text;

Task.Run(StartPublisher(2000, "error"));

Task.Run(StartPublisher(2000, "warning"));

Task.Run(StartPublisher(2000, "info"));


static Func<Task> StartPublisher(int sleepTime, string routingKey)
{
    return () =>
    {
        int counter = 0;

        do
        {
            Thread.Sleep(sleepTime);

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

                    string message = string.Format("[{1}] New message with key: {0}", routingKey, counter);

                    byte[] body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "direct_logs",
                        routingKey: routingKey,
                        body: body,
                        basicProperties: null);

                    Console.WriteLine(message + " - sent");
                }
            }
        }
        while (true);
    };
}

Console.ReadLine();