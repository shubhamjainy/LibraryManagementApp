using LibraryManagementApp.DTOs;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookAllocationService
    {
        Task<BookAllocationDto> AllocateBookAsync(BookAllocationDto bookAllocationDto);
        Task DeallocateBookAsync(BookAllocationDto bookAllocationDto);
    }
}
