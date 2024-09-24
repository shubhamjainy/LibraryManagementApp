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
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIDAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while getting book by id {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = await _bookService.AddBookAsync(bookDto);
                return CreatedAtAction("GetBookById", new { Id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while adding book {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(BookDto bookDto)
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
                _logger.LogError($"Internal server error occured while updating the book {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIDAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                await _bookService.RemoveBookAsync(book);
                return Ok("Book is deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while removing the book {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("SearchByGener/{gener:required}")]
        public async Task<IActionResult> SearchBooksByGenerAsync(string gener)
        {
            var books = await _bookService.GetBooksByGenerAsync(gener);
            return Ok(books);
        }

        [HttpGet("SearchByName/{bookNameContains:required}")]
        public async Task<IActionResult> SearchBooksByNameAsync(string bookNameContains)
        {
            var books = await _bookService.GetBooksByNameAsync(bookNameContains);
            return Ok(books);
        }

        [HttpGet("SearchByAuthorID/{id:int}")]
        public async Task<IActionResult> SearchBooksByAuthorIDAsync(int id)
        {
            var books = await _bookService.GetBooksByAuthorIDAsync(id);
            return Ok(books);
        }
    }
}
