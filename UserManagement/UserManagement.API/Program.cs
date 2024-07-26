using UserManagement.Repository;
using UserManagement.Service;
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
builder.Services.AddSingleton<RabbitMQService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
