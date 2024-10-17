using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task RemoveBookAsync(Book book);
        Task<IEnumerable<Book>> GetBooksByNameAsync(string bookNameContains);
        Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
        Task<IEnumerable<Book>> GetBooksByAuthorIDAsync(int id);
        Task<Book> GetBookByIdIncludingAuthorAsync(int id);
    }
}