using UserManagement.Repository;
using UserManagement.Service;
using MassTransit;
using FluentValidation;
using FluentValidation.AspNetCore;
using UserManagement.Service.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation konfig�rasyonu
builder.Services.AddFluentValidationAutoValidation()
                 .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();

// Ba��ml�l�k enjeksiyonlar�
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found.");
builder.Services.AddRepositories(connectionString);
builder.Services.AddServices();

// RabbitMQ ve MassTransit konfig�rasyonu
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
var host = rabbitMqSettings["Host"] ?? throw new ArgumentNullException("RabbitMQ Host not found.");
var virtualHost = rabbitMqSettings["VirtualHost"] ?? throw new ArgumentNullException("RabbitMQ VirtualHost not found.");
var username = rabbitMqSettings["Username"] ?? throw new ArgumentNullException("RabbitMQ Username not found.");
var password = rabbitMqSettings["Password"] ?? throw new ArgumentNullException("RabbitMQ Password not found.");

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(host, virtualHost, h =>
        {
            h.Username(username);
            h.Password(password);
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
