using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerService.Infrastructure;

namespace WorkerService.Consumers
{
    public class RabbitMQConsumer
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IModel _channel;

        public RabbitMQConsumer(RabbitMQConnection rabbitMQConnection, ILogger<RabbitMQConsumer> logger)
        {
            _logger = logger;
            _channel = rabbitMQConnection.GetChannel();
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received message: {message}");
            };
            _channel.BasicConsume(queue: "UserQueue", autoAck: true, consumer: consumer);
        }
    }
}
