using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace WorkerService.Infrastructure
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly ILogger<RabbitMQConnection> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConnection(IConfiguration configuration, ILogger<RabbitMQConnection> logger)
        {
            _logger = logger;
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:Host"],
                VirtualHost = configuration["RabbitMQ:VirtualHost"],
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "UserQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public IModel GetChannel()
        {
            return _channel;
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
