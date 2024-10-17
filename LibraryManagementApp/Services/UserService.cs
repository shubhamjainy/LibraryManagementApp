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
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, ILogger<UserService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CheckIfAadharNoAlreadyExistsAsync(string aadharNo)
        {
            var isAadharNoAlreadyExists = await _repository.CheckIfAadharNoAlreadyExistsAsync(aadharNo);
            return isAadharNoAlreadyExists;
        }

        public async Task<bool> CheckIfEmailAlreadyExistsAsync(string email)
        {
            var isEmailAlreadyExists = await _repository.CheckIfEmailAlreadyExistsAsync(email);
            return isEmailAlreadyExists;
        }

        public async Task<bool> CheckIfPhoneNoAlreadyExistsAsync(string phone)
        {
            var isPhoneNoAlreadyExists = await _repository.CheckIfPhoneNoAlreadyExistsAsync(phone);
            return isPhoneNoAlreadyExists;
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _repository.AddAsync(user);

            var resultDto = _mapper.Map<UserDto>(user);

            return resultDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> GetUserByIDAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<GetUserIncludingBooksAllocatedDto> GetUserByIdIncludingBooksAllocatedAsync(int id)
        {
            var user = await _repository.GetUserByIdIncludingBooksAllocatedAsync(id);
            var userDto = _mapper.Map<GetUserIncludingBooksAllocatedDto>(user);
            return userDto;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _repository.UpdateAsync(user);

            var updatedUserDto = _mapper.Map<UserDto>(user);
            return updatedUserDto;
        }
    }
}
