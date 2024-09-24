using LibraryManagementApp.DTOs;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibraryManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIDAsync(id);
                if (author == null)
                {
                    return NotFound();
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting author by id {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorDto authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var author = await _authorService.AddAuthorAsync(authorDto);
                return CreatedAtAction("GetAuthorById", new { Id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while creating author {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor(AuthorDto authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var author = await _authorService.GetAuthorByIDAsync(authorDto.Id);
                if (author == null)
                {
                    return NotFound();
                }

                var updatedAuthor = await _authorService.UpdateAuthorAsync(authorDto);
                return Ok(updatedAuthor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while updating the author {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAuthor(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIDAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                await _authorService.RemoveAuthorAsync(author);
                return Ok("Author is deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while removing the author {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
