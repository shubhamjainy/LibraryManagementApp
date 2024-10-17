using LibraryManagementApp.DTOs;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, IAuthorService authorService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting all books {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdIncludingAuthorAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting book by id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBookAsync(BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isAuthorValid = await _authorService.IsBookReferingAuthorExists(bookDto.AuthorId);
                if (!isAuthorValid)
                {
                    return BadRequest("Author id does not exists");
                }

                var book = await _bookService.AddBookAsync(bookDto);
                return CreatedAtAction(nameof(GetBookById), new { Id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while adding book {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBookAsync(BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = await _bookService.GetBookByIDAsync(bookDto.Id);
                if (book == null)
                {
                    return NotFound();
                }

                var updatedBook = await _bookService.UpdateBookAsync(bookDto);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while updating the book with id {bookDto.Id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveBookAsync(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIDAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                await _bookService.RemoveBookAsync(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while removing the book with id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("SearchByGenre/{genre:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchBooksByGenreAsync(string genre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(genre))
                {
                    return BadRequest("Genre must not be null or empty.");
                }

                var books = await _bookService.GetBooksByGenreAsync(genre);
                if (books == null || !books.Any())
                {
                    return NoContent(); // Return 204 if no books found
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while searching the book with genre {genre}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("SearchByName/{bookNameContains:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchBooksByNameAsync(string bookNameContains)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bookNameContains))
                {
                    return BadRequest("Book name must not be null or empty.");
                }

                var books = await _bookService.GetBooksByNameAsync(bookNameContains);
                if (books == null || !books.Any())
                {
                    return NoContent(); // Return 204 if no books found
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while searching the book with name {bookNameContains}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("SearchByAuthorID/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchBooksByAuthorIDAsync(int id)
        {
            try
            {
                var books = await _bookService.GetBooksByAuthorIDAsync(id);
                if (books == null || !books.Any())
                {
                    return NoContent(); // Return 204 if no books found
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while searching the book with author id {id}: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
