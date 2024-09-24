using LibraryManagementApp.DTOs;

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
        Task<IEnumerable<BookDto>> GetBooksByGenerAsync(string gener);
        Task<IEnumerable<BookDto>> GetBooksByAuthorIDAsync(int id);
    }
}
