using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();

            CreateMap<Author, GetAuthorIncludingBooksDto>();

            CreateMap<Book, GetBookIncludingAuthorDetailDto>();

            CreateMap<Book, BookDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<BookAllocation, BookAllocationDto>().ReverseMap();

            CreateMap<User, GetUserIncludingBooksAllocatedDto>();
        }
    }
}
