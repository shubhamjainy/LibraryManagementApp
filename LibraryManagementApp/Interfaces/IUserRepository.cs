using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CheckIfEmailAlreadyExistsAsync(string email);
        Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone);
        Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo);
        Task<User> GetUserByIdIncludingBooksAllocatedAsync(int id);
    }
}
