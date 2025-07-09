using AutoMapper;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using MyApp.WebMVC.Models;

namespace MyApp.WebMVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookViewModel, BookDto>().ReverseMap();
        }
    }
}
