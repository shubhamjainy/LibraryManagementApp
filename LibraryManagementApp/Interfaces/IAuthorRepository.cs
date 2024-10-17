using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task RemoveAuthorAsync(Author author);
        Task<Author> GetAuthorByIDIncludingBooksAsync(int id);
        Task<bool> IsBookReferingAuthorExists(int authorId);
    }
}
