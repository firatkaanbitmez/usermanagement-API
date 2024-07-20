using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.WorkerService.MessagingConfiguration
{
    public static class BusConfigurator
    {
        public static void ConfigureBus(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMQ");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings["Host"] ?? "defaultHost", rabbitMqSettings["VirtualHost"] ?? "/", h =>
                    {
                        h.Username(rabbitMqSettings["Username"] ?? "defaultUsername");
                        h.Password(rabbitMqSettings["Password"] ?? "defaultPassword");
                    });
                });
            });

            // services.AddMassTransitHostedService(); // Bu satırı kaldırın
        }
    }
}
