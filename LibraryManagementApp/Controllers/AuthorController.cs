using LibraryManagementApp.DTOs;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAuthorsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var authors = await _authorService.GetAllAuthorsAsync(pageNumber, pageSize);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting all the authors on pageNumber {pageNumber} and pageSize {pageSize} {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                _logger.LogError($"Internal server error occured while getting author by id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("GetAuthorIncludingBooks/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorByIdIncludingBooksAsync(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIDIncludingBooksAsync(id);
                if (author == null)
                {
                    return NotFound();
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting author details by id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuthorAsync(AuthorDto authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var author = await _authorService.AddAuthorAsync(authorDto);
                return CreatedAtAction(nameof(GetAuthorById), new { Id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while creating author {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthorAsync(AuthorDto authorDto)
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
                _logger.LogError($"Internal server error occured while updating the author with id {authorDto.Id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAuthorAsync(int id)
        {
            try
            {
                var result = await _authorService.DeleteAuthorAndBooksAsync(id);
                if (!result)
                {
                    return NotFound("Author not found.");
                }

                return Ok("Author and their books deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while removing the author with id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
