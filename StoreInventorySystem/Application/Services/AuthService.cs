using Microsoft.AspNetCore.Identity;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> RegisterAsync(string username, string password)
        {
            var existing = await _userRepository.GetByUsername(username);
            
            if (existing != null)
                return null;

            var user = new User
            {
                Username = username,
                Role = "User"
            };

            user.PasswordHash = _hasher.HashPassword(user, password);

            await _userRepository.Add(user);

            return user;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);

            if(user == null)
                return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}
