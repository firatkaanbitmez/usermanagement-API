using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.WorkerService.Consumers;

namespace UserManagement.WorkerService.MessagingConfiguration
{
    public static class BusConfigurator
    {
        public static void ConfigureBus(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMQ");
            var host = rabbitMqSettings["Host"] ?? throw new ArgumentNullException("RabbitMQ Host not found.");
            var virtualHost = rabbitMqSettings["VirtualHost"] ?? throw new ArgumentNullException("RabbitMQ VirtualHost not found.");
            var username = rabbitMqSettings["Username"] ?? throw new ArgumentNullException("RabbitMQ Username not found.");
            var password = rabbitMqSettings["Password"] ?? throw new ArgumentNullException("RabbitMQ Password not found.");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, virtualHost, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("user_queue", e =>
                    {
                        e.ConfigureConsumer<UserConsumer>(context);
                    });
                });
            });
        }
    }
}
