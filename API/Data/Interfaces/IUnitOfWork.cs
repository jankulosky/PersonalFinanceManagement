namespace API.Data.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
