using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.WorkerService;
using UserManagement.WorkerService.MessagingConfiguration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.ConfigureBus(context.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
