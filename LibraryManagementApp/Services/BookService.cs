using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace LibraryManagementApp.Services
{
    public class BookService : IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        string cacheKey = $"Books";

        public BookService(IBookRepository repository, ILogger<BookService> logger, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BookDto> AddBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _repository.AddAsync(book);

            var resultDto = _mapper.Map<BookDto>(book);

            return resultDto;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            IEnumerable<Book> books = new List<Book>();

            var cachedBook = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedBook))
            {
                // Deserialize the cached book data if found
                books = JsonConvert.DeserializeObject<List<Book>>(cachedBook);
            }
            books = await _repository.GetAllAsync();
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            // Cache the book data for future requests
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Absolute expiration
                SlidingExpiration = TimeSpan.FromMinutes(2) // Sliding expiration
            };

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(books), cacheEntryOptions);
            return booksDto;
        }

        public async Task<BookDto> GetBookByIDAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByAuthorIDAsync(int id)
        {
            var books = await _repository.GetBooksByAuthorIDAsync(id);
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByGenreAsync(string genre)
        {
            var books = await _repository.GetBooksByGenreAsync(genre);
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task<IEnumerable<BookDto>> GetBooksByNameAsync(string bookNameContains)
        {
            var books = await _repository.GetBooksByNameAsync(bookNameContains);
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDto;
        }

        public async Task RemoveBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _repository.RemoveBookAsync(book);
        }

        public async Task<BookDto> UpdateBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _repository.UpdateAsync(book);

            var updatedBookDto = _mapper.Map<BookDto>(book);
            return updatedBookDto;
        }

        public async Task<GetBookIncludingAuthorDetailDto> GetBookByIdIncludingAuthorAsync(int id)
        {
            var book = await _repository.GetBookByIdIncludingAuthorAsync(id);
            var bookDto = _mapper.Map<GetBookIncludingAuthorDetailDto>(book);
            return bookDto;
        }
    }
}
