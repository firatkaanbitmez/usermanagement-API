using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Core.Interfaces;
using UserManagement.Repository.Data;
using UserManagement.Repository.Repositories;

namespace UserManagement.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserManagementDbContext>(options =>
                options.UseSqlServer(connectionString));

            // UnitOfWork'ün tam yolunu kullanın
            services.AddScoped<IUnitOfWork, UserManagement.Repository.UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
