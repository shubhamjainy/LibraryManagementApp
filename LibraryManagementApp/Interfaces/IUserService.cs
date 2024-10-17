using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(UserDto userDto);
        Task<UserDto> GetUserByIDAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> CheckIfEmailAlreadyExistsAsync(string email);
        Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone);
        Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo);
        Task<GetUserIncludingBooksAllocatedDto> GetUserByIdIncludingBooksAllocatedAsync(int id);
    }
}
