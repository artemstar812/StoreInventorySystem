using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);

        Task Add(User user);
    }
}
