using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<List<Book>> GetBooksByUserIdAsync(Guid userId);
    }
}
