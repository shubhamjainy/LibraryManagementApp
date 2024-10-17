using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly LibraryDbContext _dbContext;

        public UserRepository(LibraryDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(a => a.AadharNo == aadharNo);
        }

        public async Task<bool> CheckIfEmailAlreadyExistsAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(a => a.Email == email);
        }

        public async Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(a => a.PhoneNo == phone);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<User> GetUserByIdIncludingBooksAllocatedAsync(int id)
        {
            return await _dbContext.Users.Include(a => a.BooksAllocated).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
