using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> AddAuthorAsync(AuthorDto author);
        Task RemoveAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(AuthorDto author);
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto> GetAuthorByIDAsync(int id);
    }
}
