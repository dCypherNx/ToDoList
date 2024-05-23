using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WebAPI.Infrastructure.Messaging
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly ConnectionFactory _factory;
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;

        public RabbitMQMessagePublisher(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMQ:HostName"];
            _username = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];
            _port = int.Parse(configuration["RabbitMQ:Port"]);

            _factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                Port = _port
            };
        }

        public void Publish(string message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "taskQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "taskQueue",
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
