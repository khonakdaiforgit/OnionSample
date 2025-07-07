using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data;


namespace MyApp.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksByUserIdAsync(Guid userId)
        {
            return await _context.Books.Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
