using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly LibraryDbContext _dbContext;

        public AuthorRepository(LibraryDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Author> GetAuthorByIDIncludingBooksAsync(int id)
        {
            return await _dbContext.Authors.Include(a => a.Books).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task RemoveAuthorAsync(Author author)
        {
            _dbContext.Authors.Remove(author);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsBookReferingAuthorExists(int authorId)
        {
            // Check if the referenced Author exists
            return await _dbContext.Authors.AnyAsync(a => a.Id == authorId);
        }
    }
}
