using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(u => u.DateAdded >= startDate && u.DateAdded <= endDate).ToListAsync();
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
