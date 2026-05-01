using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}