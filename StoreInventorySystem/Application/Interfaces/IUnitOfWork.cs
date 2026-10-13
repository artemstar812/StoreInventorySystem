namespace InventorySystem.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
        Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, 
            CancellationToken cancellationToken = default);
    }
}
