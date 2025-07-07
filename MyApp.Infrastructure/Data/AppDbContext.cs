using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
