using InventorySystem.Application.Interfaces;
using StoreInventorySystem.Infrastructure;

namespace InventorySystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);   

            try
            {
                await operation(cancellationToken);

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);

                throw;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            return _dbContext.SaveChangesAsync(cancellation);
        }
    }
}
