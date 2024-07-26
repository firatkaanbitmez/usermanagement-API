using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate).ToListAsync();
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            return await _dbSet.CountAsync(u => u.IsActive);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync()
        {
            return await _dbSet.Where(u => !u.IsActive).ToListAsync();
        }
    }
}
