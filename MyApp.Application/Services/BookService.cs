using AutoMapper;
using MyApp.Application.DTOs;
using MyApp.Application.Interface;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookService(
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task AddBookAsync(Guid userId, AddBookDto dto)
        {
            //var book = new Book(dto.Title, dto.Publisher, userId);
            //await _bookRepository.AddAsync(book);
            var book = _mapper.Map<Book>(dto);
            book.UserId = userId;
            await _bookRepository.AddAsync(book);
        }

        public async Task<List<BookDto>> GetUserBooksAsync(Guid userId)
        {
            var books = await _bookRepository.GetBooksByUserIdAsync(userId);
            return _mapper.Map<List<BookDto>>(books);
        }
    }
}
