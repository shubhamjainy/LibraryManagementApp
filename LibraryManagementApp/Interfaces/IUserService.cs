using LibraryManagementApp.DTOs;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(UserDto userDto);
        Task<UserDto> GetUserByIDAsnyc(int id);
        Task<UserDto> GetUserByEmailAsnyc(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> CheckIfEmailAlreadyExistsAsync(string email);
        Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone);
        Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo);
    }
}
