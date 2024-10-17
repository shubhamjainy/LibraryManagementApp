using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.UnitOfWorkPattern;

namespace LibraryManagementApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ILogger<AuthorService> _logger;
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IAuthorRepository repository, IUnitOfWork unitOfWork, ILogger<AuthorService> logger, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AuthorDto> AddAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            var addedAuthor = await _repository.AddAsync(author);

            var resultDto = _mapper.Map<AuthorDto>(addedAuthor);

            return resultDto;
        }

        public async Task<bool> DeleteAuthorAndBooksAsync(int authorId)
        {
            // Check if the author exists
            var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
            if (author == null)
            {
                return false; // Author not found
            }

            // Get all books by the author
            var books = await _unitOfWork.Books.GetBooksByAuthorIDAsync(authorId);

            // Delete all books of the author
            foreach (var book in books)
            {
                await _unitOfWork.Books.RemoveBookAsync(book);
            }

            // Delete the author
           await _unitOfWork.Authors.RemoveAuthorAsync(author);

            // Commit the transaction
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize)
        {
            var authors = await _repository.GetAllAsync(pageNumber, pageSize);
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return authorsDto;
        }

        public async Task<AuthorDto> GetAuthorByIDAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        public async Task<GetAuthorIncludingBooksDto> GetAuthorByIDIncludingBooksAsync(int id)
        {
            var author = await _repository.GetAuthorByIDIncludingBooksAsync(id);
            var authorDto = _mapper.Map<GetAuthorIncludingBooksDto>(author);
            return authorDto;
        }

        public async Task<bool> IsBookReferingAuthorExists(int authorId)
        {
            return await _repository.IsBookReferingAuthorExists(authorId);
        }

        public async Task RemoveAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _repository.RemoveAuthorAsync(author);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _repository.UpdateAsync(author);
            var updatedAuthorDto = _mapper.Map<AuthorDto>(author);
            return updatedAuthorDto;
        }
    }
}
