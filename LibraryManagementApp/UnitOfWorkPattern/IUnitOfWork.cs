using LibraryManagementApp.Interfaces;

namespace LibraryManagementApp.UnitOfWorkPattern
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        Task<int> SaveChangesAsync();
    }
}
