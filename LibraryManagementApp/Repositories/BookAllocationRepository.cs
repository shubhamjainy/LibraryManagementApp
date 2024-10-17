using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class BookAllocationRepository : IBookAllocationRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BookAllocationRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookAllocation> AllocateBookAsync(BookAllocation bookAllocation)
        {
            await _dbContext.BookAllocations.AddAsync(bookAllocation);
            await _dbContext.SaveChangesAsync();
            return bookAllocation;
        }

        public async Task DeallocateBookAsync(BookAllocation bookAllocation)
        {
            var item = await _dbContext.BookAllocations.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == bookAllocation.UserId && a.BookId == bookAllocation.BookId);
            _dbContext.BookAllocations.Remove(item);

            await _dbContext.SaveChangesAsync();
        }
    }
}
