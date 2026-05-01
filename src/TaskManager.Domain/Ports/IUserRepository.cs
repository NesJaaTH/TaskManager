using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface IUserRepository
    {
        Task<User?> FindByIdAsync(Guid id);
        Task<User?> FindByEmailAsync(string email);
        Task<User> CreateAsync (User user);
    }
}
