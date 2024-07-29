using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerService.Infrastructure;

namespace WorkerService.Consumers
{
    public class RabbitMQUserConsumer
    {
        private readonly ILogger<RabbitMQUserConsumer> _logger;
        private readonly IModel _channel;

        public RabbitMQUserConsumer(RabbitMQConnection rabbitMQConnection, ILogger<RabbitMQUserConsumer> logger)
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
