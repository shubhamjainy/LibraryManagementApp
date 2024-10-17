using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookAllocationRepository
    {
        Task<BookAllocation> AllocateBookAsync(BookAllocation bookAllocation);
        Task DeallocateBookAsync(BookAllocation bookAllocation);
    }
}