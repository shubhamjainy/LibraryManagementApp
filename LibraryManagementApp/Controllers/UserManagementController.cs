using LibraryManagementApp.DTOs;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserService userService, ILogger<UserManagementController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIDAsnyc(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by id {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("Email/{email:required}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsnyc(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by email {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _userService.CheckIfEmailAlreadyExistsAsync(userDto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return BadRequest(ModelState);
                }

                if (await _userService.CheckIfPhoneNoAlreadyExistsAsync(userDto.PhoneNo))
                {
                    ModelState.AddModelError("PhoneNo", "Phone no. is already registered.");
                    return BadRequest(ModelState);
                }

                if (await _userService.CheckIfAadharNoAlreadyExistsAsync(userDto.AadharNo))
                {
                    ModelState.AddModelError("AadharNo", "AadharNo mentioned is already used.");
                    return BadRequest(ModelState);
                }

                var user = await _userService.CreateUserAsync(userDto);
                return CreatedAtAction("GetUserById", new { Id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while creating user {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetUserByIDAsnyc(userUpdateDto.Id);
                if (user == null)
                {
                    return NotFound();
                }

                if (await _userService.CheckIfPhoneNoAlreadyExistsAsync(userUpdateDto.PhoneNo)
                    && user.PhoneNo != userUpdateDto.PhoneNo)
                {
                    ModelState.AddModelError("PhoneNo", "Phone no. is already registered with some other account.");
                    return BadRequest(ModelState);
                }

                UserDto finalUserDto = new UserDto()
                {
                    Id = userUpdateDto.Id,
                    Name = userUpdateDto.Name,
                    PhoneNo = userUpdateDto.PhoneNo,
                    Address = userUpdateDto.Address,
                    AadharNo = user.AadharNo,
                    Email = user.Email
                };

                var updatedUser = await _userService.UpdateUserAsync(finalUserDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while updating the user {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpGet("BookAllocation")]
        public async Task<IActionResult> AllocateBookToUser(BookAllocationDto bookAllocationDto)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsnyc(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by email {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("BookDeallocation")]
        public async Task<IActionResult> DeallocateBookToUser(BookAllocationDto bookAllocationDto)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsnyc(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by email {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
