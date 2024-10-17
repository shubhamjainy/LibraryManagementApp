using LibraryManagementApp.Interfaces;
using LibraryManagementApp.UnitOfWorkPattern;

namespace LibraryManagementApp.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _dbContext;

        public UnitOfWork(LibraryDbContext dbContext, IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            Authors = authorRepository;
            Books = bookRepository;
        }
        public IAuthorRepository Authors { get; }

        public IBookRepository Books { get; }

        public async Task<int> SaveChangesAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }
    }
}
