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
        private readonly IBookService _bookService;
        private readonly IBookAllocationService _bookAllocationService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserService userService, IBookService bookService, IBookAllocationService bookAllocationService, ILogger<UserManagementController> logger)
        {
            _userService = userService;
            _bookService = bookService;
            _bookAllocationService = bookAllocationService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting all the users {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByIdIncludingAllocatedBooks(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdIncludingBooksAllocatedAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by id {id} including books: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("Email/{email:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting user by email {email}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync(UserDto userDto)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetUserByIDAsync(userUpdateDto.Id);
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
                _logger.LogError($"Internal server error occured while updating the user with id {userUpdateDto.Id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpPost("BookAllocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AllocateBookToUserAsync(BookAllocationDto bookAllocationDto)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(bookAllocationDto.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var book = await _bookService.GetBookByIDAsync(bookAllocationDto.BookId);
                if (book == null)
                {
                    return NotFound();
                }
                var allocationDetails = await _bookAllocationService.AllocateBookAsync(bookAllocationDto);

                return Ok(allocationDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting allocating book to a user {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("BookDeallocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeallocateBookToUserAsync(BookAllocationDto bookAllocationDto)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(bookAllocationDto.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var book = await _bookService.GetBookByIDAsync(bookAllocationDto.BookId);
                if (book == null)
                {
                    return NotFound();
                }
                await _bookAllocationService.DeallocateBookAsync(bookAllocationDto);

                return Ok("Book deallocated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting deallocating book from a user {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
