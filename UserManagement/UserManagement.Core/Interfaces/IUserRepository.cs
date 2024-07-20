using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Entities;

namespace UserManagement.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<int> GetActiveUserCountAsync();
    }
}
