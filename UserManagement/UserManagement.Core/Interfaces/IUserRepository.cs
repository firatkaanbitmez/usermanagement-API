using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.Entities;

namespace UserManagement.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<int> GetActiveUserCountAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetInactiveUsersAsync();
    }
}
