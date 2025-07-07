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

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task AddBookAsync(Guid userId, AddBookDto dto)
        {
            var book = new Book(dto.Title, dto.Publisher, userId);
            await _bookRepository.AddAsync(book);
        }

        public async Task<List<Book>> GetUserBooksAsync(Guid userId)
        {
            return await _bookRepository.GetBooksByUserIdAsync(userId);
        }
    }
}
