using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;

namespace LibraryManagementApp.Services
{
    public class BookAllocationService : IBookAllocationService
    {
        private readonly ILogger<BookAllocationService> _logger;
        private readonly IBookAllocationRepository _repository;
        private readonly IMapper _mapper;

        public BookAllocationService(IBookAllocationRepository repository, ILogger<BookAllocationService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookAllocationDto> AllocateBookAsync(BookAllocationDto bookAllocationDto)
        {
            var bookAllocation = _mapper.Map<BookAllocation>(bookAllocationDto);
            bookAllocation.AllocationDate = DateTime.Now;
            bookAllocation.ReturnDate = DateTime.Now.AddDays(7);

            await _repository.AllocateBookAsync(bookAllocation);

            var resultDto = _mapper.Map<BookAllocationDto>(bookAllocation);

            return resultDto;
        }

        public async Task DeallocateBookAsync(BookAllocationDto bookAllocationDto)
        {
            var bookAllocation = _mapper.Map<BookAllocation>(bookAllocationDto);

            await _repository.DeallocateBookAsync(bookAllocation);
        }
    }
}
