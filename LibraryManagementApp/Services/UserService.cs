using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(LibraryDbContext context, ILogger<UserService> logger, IMapper mapper)
        {
            _dbContext = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo)
        {
            var isAadharNoAlreadyExists = await _dbContext.Users.AsNoTracking().AnyAsync(a => a.AadharNo == aadharNo);
            return isAadharNoAlreadyExists;
        }

        public async Task<bool> CheckIfEmailAlreadyExistsAsync(string email)
        {
            var isEmailAlreadyExists = await _dbContext.Users.AsNoTracking().AnyAsync(a => a.Email == email);
            return isEmailAlreadyExists;
        }

        public async Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone)
        {
            var isPhoneNoAlreadyExists = await _dbContext.Users.AsNoTracking().AnyAsync(a => a.PhoneNo == phone);
            return isPhoneNoAlreadyExists;
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var resultDto = _mapper.Map<UserDto>(user);

            return resultDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetUserByEmailAsnyc(string email)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> GetUserByIDAsnyc(int id)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();

            var updatedUserDto = _mapper.Map<UserDto>(user);
            return updatedUserDto;
        }
    }
}
