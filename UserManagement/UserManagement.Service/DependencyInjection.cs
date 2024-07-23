using Microsoft.Extensions.DependencyInjection;
using UserManagement.Service.Services;
using UserManagement.Service.MappingProfiles;

namespace UserManagement.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile));

            services.AddScoped<UserService>();

            return services;
        }
    }
}
