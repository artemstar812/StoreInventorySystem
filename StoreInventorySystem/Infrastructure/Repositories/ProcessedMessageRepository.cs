using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StoreInventorySystem.Infrastructure;

namespace InventorySystem.Infrastructure.Repositories
{
    public class ProcessedMessageRepository : IProcessedMessagesRepository
    {
        private readonly AppDbContext _dbContext;

        public ProcessedMessageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ProcessedMessage message)
        {
            await _dbContext.ProcessedMessages.AddAsync(message);
        }

        public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return await _dbContext.ProcessedMessages.AnyAsync(e => e.EventId == eventId, cancellationToken);
        }
    }
}
