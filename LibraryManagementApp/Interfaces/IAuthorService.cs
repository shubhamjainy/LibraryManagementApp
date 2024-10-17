using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthorAsync(AuthorDto author);
        Task RemoveAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(AuthorDto author);
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize);
        Task<AuthorDto> GetAuthorByIDAsync(int id);
        Task<GetAuthorIncludingBooksDto> GetAuthorByIDIncludingBooksAsync(int id);
        Task<bool> IsBookReferingAuthorExists(int authorId);
        Task<bool> DeleteAuthorAndBooksAsync(int authorId);
    }
}
