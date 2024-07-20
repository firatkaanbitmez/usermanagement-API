using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.WorkerService;
using UserManagement.WorkerService.MessagingConfiguration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // RabbitMQ ve MassTransit konfigürasyonu
        services.ConfigureBus(context.Configuration);

        // Worker servisini ekle
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
