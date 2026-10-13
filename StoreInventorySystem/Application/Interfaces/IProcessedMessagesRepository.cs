using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces
{
    public interface IProcessedMessagesRepository
    {
        Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken);

        Task AddAsync(ProcessedMessage message);
    }
}
