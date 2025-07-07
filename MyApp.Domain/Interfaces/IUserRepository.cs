using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces
{


    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByUsernameAsync(string username); // متد جدید
        Task AddAsync(User user);
    }
}
