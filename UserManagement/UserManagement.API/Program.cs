using UserManagement.Repository;
using UserManagement.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using UserManagement.Service.Validators;
using UserManagement.Core.Interfaces;
using UserManagement.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation()
                 .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found.");
builder.Services.AddRepositories(connectionString);
builder.Services.AddServices();
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddScoped<IUserChangeDetector, UserChangeDetector>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
