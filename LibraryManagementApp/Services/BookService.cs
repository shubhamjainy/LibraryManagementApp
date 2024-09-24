using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace LibraryManagementApp.Services
{
    public class BookService : IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public BookService(LibraryDbContext context, ILogger<BookService> logger, IMapper mapper)
        {
            _dbContext = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto> AddBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            var resultDto = _mapper.Map<BookDto>(book);

            return resultDto;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _dbContext.Books.ToListAsync();
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return booksDto;
        }

        public async Task<BookDto> GetBookByIDAsync(int id)
        {
            var book = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByAuthorIDAsync(int id)
        {
            var books = await _dbContext.Books.Where(a => a.AuthorId == id).ToListAsync();
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByGenerAsync(string gener)
        {
            var books = await _dbContext.Books.Where(a => a.Genre.Contains(gener)).ToListAsync();
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByNameAsync(string bookNameContains)
        {
            var books = await _dbContext.Books.Where(a => a.Name.Contains(bookNameContains)).ToListAsync();
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task RemoveBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<BookDto> UpdateBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _dbContext.Books.Update(book);

            await _dbContext.SaveChangesAsync();

            var updatedBookDto = _mapper.Map<BookDto>(book);
            return updatedBookDto;
        }
    }
}
