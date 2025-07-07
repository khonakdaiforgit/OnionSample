using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Interface
{
    public interface IBookService
    {
        Task AddBookAsync(Guid userId, AddBookDto dto);
        Task<List<Book>> GetUserBooksAsync(Guid userId);
    }
}
