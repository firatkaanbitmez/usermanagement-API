using UserManagement.Repository;
using UserManagement.Service;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using UserManagement.WorkerService.Consumers;
using UserManagement.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Baðýmlýlýk enjeksiyonlarý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found.");
builder.Services.AddRepositories(connectionString);
builder.Services.AddServices();

// RabbitMQ ve MassTransit konfigürasyonu
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
var host = rabbitMqSettings["Host"] ?? throw new ArgumentNullException("RabbitMQ Host not found.");
var virtualHost = rabbitMqSettings["VirtualHost"] ?? throw new ArgumentNullException("RabbitMQ VirtualHost not found.");
var username = rabbitMqSettings["Username"] ?? throw new ArgumentNullException("RabbitMQ Username not found.");
var password = rabbitMqSettings["Password"] ?? throw new ArgumentNullException("RabbitMQ Password not found.");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>(); // UserConsumer'ý ekliyoruz
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

var app = builder.Build();

// Global Exception Handler middleware
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
