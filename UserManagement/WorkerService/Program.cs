using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService;
using WorkerService.Consumers;
using WorkerService.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<RabbitMQConnection>();
        services.AddSingleton<RabbitMQUserConsumer>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
