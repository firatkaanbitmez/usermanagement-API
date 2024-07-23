using Microsoft.Extensions.DependencyInjection;
using UserManagement.Service.Services;
using UserManagement.Service.MappingProfiles;
using UserManagement.Core.Interfaces;

namespace UserManagement.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile));
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
