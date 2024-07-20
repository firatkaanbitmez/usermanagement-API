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

![Proje Yapısı](https://raw.githubusercontent.com/firatkaanbitmez/usermanagement-API/main/Project-Documents/20.07-katmanyapisi.png)

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