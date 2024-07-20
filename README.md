# User Management API

Bu proje, .NET 8, EF Core ve RabbitMQ/MassTransit kullanarak oluşturulmuş bir User Management API'dir. Proje katmanlı mimariyi takip eder ve kullanıcılarla ilgili mesajları asenkron olarak işlemek için bir worker service içerir.

## Kurulum

1. Repository'i klonlayın:
    ```sh
    git clone https://github.com/firatkaanbitmez/usermanagement-API.git
    ```
2. Proje dizinine gidin:
    ```sh
    cd usermanagement-api/UserManagement
    ```
3. Bağımlılıkları yükleyin:
    ```sh
    dotnet restore
    ```
4. Veritabanı göçlerini uygulayın:
    ```sh
    dotnet ef database update -p UserManagement.Repository -s UserManagement.API
    ```
5. `appsettings.json` dosyasındaki RabbitMQ bilgilerini güncelleyin.

## Kullanım

1. API projesini çalıştırın:
    ```sh
    dotnet run --project UserManagement.API
    ```
2. Worker Service projesini çalıştırın:
    ```sh
    dotnet run --project UserManagement.WorkerService
    ```

## Proje Yapısı

UserManagement
├── UserManagement.API # API Katmanı
│ ├── Controllers # Kontrolörler
│ ├── Filters # Filtreler (hata yönetimi ve yetkilendirme)
│ ├── Middleware # Ara Katmanlar
│ ├── appsettings.json # Yapılandırma Dosyaları
│ └── Program.cs # Uygulama Başlangıç Ayarları
├── UserManagement.Core # Çekirdek/Uygulama Katmanı
│ ├── DTOs # Data Transfer Object'ler
│ ├── Entities # Veritabanı Varlıkları
│ ├── Interfaces # Arayüzler
│ ├── Services # Servis Arayüzleri
│ └── Mappings # Nesne Dönüştürme Ayarları
├── UserManagement.Repository # Repository Katmanı
│ ├── Data # Veritabanı Bağlamı
│ ├── Configurations # Varlık Yapılandırmaları
│ ├── Migrations # Veritabanı Göçleri
│ ├── Repositories # Veri Erişim Sınıfları
│ ├── UnitOfWork # Birim Çalışma Implementasyonları
│ └── DependencyInjection.cs # Bağımlılık Enjeksiyonu
├── UserManagement.Service # Servis Katmanı
│ ├── Services # İş Mantığı Servisleri
│ ├── Messaging # RabbitMQ ve MassTransit yapılandırmaları ve işleyicileri
│ ├── MappingProfiles # AutoMapper Profilleri
│ └── DependencyInjection.cs # Bağımlılık Enjeksiyonu
└── UserManagement.WorkerService # Worker Servis Katmanı
├── Program.cs # Worker Servis Giriş Noktası
├── Worker.cs # İşleyici Sınıfı
├── Consumers # RabbitMQ Tüketicileri
└── MessagingConfiguration # RabbitMQ ve MassTransit Yapılandırmaları


## Endpointler

- **POST /api/User**: Yeni bir kullanıcı ekler.
- **GET /api/User/{id}**: Belirli bir kullanıcıyı getirir.
- **PUT /api/User/{id}**: Bir kullanıcıyı günceller.
- **DELETE /api/User/{id}**: Bir kullanıcıyı siler.
- **GET /api/User/active**: Aktif kullanıcı sayısını getirir.
- **GET /api/User/added**: Belirli bir tarih aralığında eklenen kullanıcıları getirir.

## Worker Service'in Çalıştırılması

Worker service'i çalıştırmak için RabbitMQ'nun çalışır durumda olduğundan ve `appsettings.json` dosyanızda doğru şekilde yapılandırıldığından emin olun. Ardından şu komutu çalıştırın:

```sh
dotnet run --project UserManagement.WorkerService
```

## Kullanılan Teknolojiler
- ** .NET 8
- ** Entity Framework Core
- ** RabbitMQ
- ** MassTransit
- ** AutoMapper