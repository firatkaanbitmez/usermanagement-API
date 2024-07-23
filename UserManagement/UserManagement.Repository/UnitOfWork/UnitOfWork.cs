using UserManagement.Core.Interfaces;
using UserManagement.Repository.Data;
using UserManagement.Repository.Repositories;

namespace UserManagement.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManagementDbContext _context;
        private UserRepository? _userRepository;

        public UnitOfWork(UserManagementDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
