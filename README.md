# User Management API

This project is a User Management API built with .NET 8, EF Core, and RabbitMQ/MassTransit. The project follows a layered architecture and includes a worker service for handling user-related messages asynchronously.

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/usermanagement-api.git
    ```
2. Navigate to the project directory:
    ```sh
    cd usermanagement-api/UserManagement
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```
4. Apply the database migrations:
    ```sh
    dotnet ef database update -p UserManagement.Repository -s UserManagement.API
    ```
5. Update `appsettings.json` with your RabbitMQ credentials.

## Usage

1. Run the API project:
    ```sh
    dotnet run --project UserManagement.API
    ```
2. Run the Worker Service project:
    ```sh
    dotnet run --project UserManagement.WorkerService
    ```

## Project Structure

UserManagement
├── UserManagement.API # API Katmanı
│   ├── Controllers # Kontrolörler
│   ├── Filters # Filtreler (hata yönetimi ve yetkilendirme)
│   ├── Middleware # Ara Katmanlar
│   ├── appsettings.json # Yapılandırma Dosyaları
│   └── Program.cs # Uygulama Başlangıç Ayarları
│
├── UserManagement.Core # Çekirdek/Uygulama Katmanı
│   ├── DTOs # Data Transfer Object'ler
│   ├── Entities # Veritabanı Varlıkları
│   ├── Interfaces # Arayüzler
│   ├── Services # Servis Arayüzleri
│   └── Mappings # Nesne Dönüştürme Ayarları
│
├── UserManagement.Repository # Repository Katmanı
│   ├── Data # Veritabanı Bağlamı
│   ├── Configurations # Varlık Yapılandırmaları
│   ├── Migrations # Veritabanı Göçleri
│   ├── Repositories # Veri Erişim Sınıfları
│   ├── UnitOfWork # Birim Çalışma Implementasyonları
│   └── DependencyInjection.cs # Bağımlılık Enjeksiyonu
│
├── UserManagement.Service # Servis Katmanı
│   ├── Services # İş Mantığı Servisleri
│   ├── Messaging # RabbitMQ ve MassTransit yapılandırmaları ve işleyicileri
│   ├── MappingProfiles # AutoMapper Profilleri
│   └── DependencyInjection.cs # Bağımlılık Enjeksiyonu
│
└── UserManagement.WorkerService # Worker Servis Katmanı
    ├── Program.cs # Worker Servis Giriş Noktası
    ├── Worker.cs # İşleyici Sınıfı
    ├── Consumers # RabbitMQ Tüketicileri
    └── MessagingConfiguration # RabbitMQ ve MassTransit Yapılandırmaları

## Endpoints

- **POST /api/User**: Add a new user.
- **GET /api/User/{id}**: Get a user by ID.
- **PUT /api/User/{id}**: Update a user.
- **DELETE /api/User/{id}**: Delete a user.
- **GET /api/User/active**: Get the count of active users.
- **GET /api/User/added**: Get users added within a specific date range.

## Running the Worker Service

To run the worker service, ensure that RabbitMQ is running and properly configured in your `appsettings.json`. Then, execute:

```sh
dotnet run --project UserManagement.WorkerService
```

## Technologies Used
- ** .NET 8
- ** Entity Framework Core
- ** RabbitMQ
- ** MassTransit
- ** AutoMapper