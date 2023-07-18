using API.DTOs;

namespace API.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionDto>> GetTransactionList(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize);
    }
}
