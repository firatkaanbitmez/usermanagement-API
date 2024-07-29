using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.Consumers;
using WorkerService.Infrastructure;
using WorkerService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<RabbitMQConnection>();
        services.AddSingleton<RabbitMQConsumer>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
