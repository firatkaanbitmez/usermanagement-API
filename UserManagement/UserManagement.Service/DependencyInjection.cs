using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using UserManagement.Service.Services;
using UserManagement.Service.MappingProfiles;

namespace UserManagement.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // AutoMapper'ı aşağıdaki gibi ekleyin
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            services.AddScoped<UserService>();

            return services;
        }
    }
}
