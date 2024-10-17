using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BookRepository(LibraryDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Book> GetBookByIdIncludingAuthorAsync(int id)
        {
            return await _dbContext.Books.Include(a => a.Author).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIDAsync(int id)
        {
            return await _dbContext.Books.Where(a => a.AuthorId == id).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
        {
            return await _dbContext.Books.Where(a => a.Genre.Contains(genre)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByNameAsync(string bookNameContains)
        {
            return await _dbContext.Books.Where(a => a.Name.Contains(bookNameContains)).ToListAsync();
        }

        public async Task RemoveBookAsync(Book book)
        {
            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();
        }
    }
}
