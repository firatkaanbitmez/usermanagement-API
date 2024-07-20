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

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings["Host"], rabbitMqSettings["VirtualHost"], h =>
                    {
                        h.Username(rabbitMqSettings["Username"]);
                        h.Password(rabbitMqSettings["Password"]);
                    });

                    cfg.ReceiveEndpoint("user_queue", e =>
                    {
                        e.ConfigureConsumer<UserConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
