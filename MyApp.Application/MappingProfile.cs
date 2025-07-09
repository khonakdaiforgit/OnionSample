using AutoMapper;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;


namespace MyApp.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Book, AddBookDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
