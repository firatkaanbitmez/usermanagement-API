using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.WorkerService.MessagingConfiguration
{
    public static class BusConfigurator
    {
        public static void ConfigureBus(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
                });
            });

            // services.AddMassTransitHostedService(); // Bu satırı kaldırın
        }
    }
}
