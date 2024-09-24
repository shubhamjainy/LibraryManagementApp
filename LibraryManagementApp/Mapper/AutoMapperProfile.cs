using AutoMapper;
using LibraryManagementApp.DTOs;
using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Mapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, AuthorDto>();
              CreateMap<AuthorDto, Author>();
        }
    }
}
