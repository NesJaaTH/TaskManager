using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwt;

        public UserService(IUserRepository userRepo, IPasswordHasher passwordHasher, IJwtService tokenService)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _jwt = tokenService;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.FindByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email already in use.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            var created = await _userRepo.CreateAsync(user);
            return new UserDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.FindByEmailAsync(dto.Email)
                ?? throw new InvalidOperationException("Invalid email or password.");

            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password.");
            return _jwt.GenerateToken(user);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepo.FindByIdAsync(id)
                ?? throw new InvalidOperationException("User not found.");
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
