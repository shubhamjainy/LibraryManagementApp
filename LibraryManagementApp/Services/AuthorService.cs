using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ILogger<AuthorService> _logger;
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorService(LibraryDbContext context, ILogger<AuthorService> logger, IMapper mapper)
        {
            _dbContext = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AuthorDto> AddAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            var resultDto = _mapper.Map<AuthorDto>(author);

            return resultDto;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _dbContext.Authors.ToListAsync();
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return authorsDto;
        }

        public async Task<AuthorDto> GetAuthorByIDAsync(int id)
        {
            var author = await _dbContext.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        public async Task RemoveAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _dbContext.Authors.Remove(author);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<AuthorDto> UpdateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _dbContext.Authors.Update(author);

            //// Attach the entity and mark it as modified
            //_dbContext.Authors.Attach(author);
            //_dbContext.Entry(author).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            var updatedAuthorDto = _mapper.Map<AuthorDto>(author);
            return updatedAuthorDto;
        }
    }
}
