using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> AddBookAsync(BookDto bookDto);
        Task RemoveBookAsync(BookDto bookDto);
        Task<BookDto> UpdateBookAsync(BookDto bookDto);
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto> GetBookByIDAsync(int id);
        Task<IEnumerable<BookDto>> GetBooksByNameAsync(string bookNameContains);
        Task<IEnumerable<BookDto>> GetBooksByGenreAsync(string genre);
        Task<IEnumerable<BookDto>> GetBooksByAuthorIDAsync(int id);
        Task<GetBookIncludingAuthorDetailDto> GetBookByIdIncludingAuthorAsync(int id);
    }
}
