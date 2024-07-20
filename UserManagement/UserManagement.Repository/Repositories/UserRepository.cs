using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using UserManagement.Repository.Data;

namespace UserManagement.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UserManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .Where(u => u.DateAdded >= startDate && u.DateAdded <= endDate)
                .ToListAsync();
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
