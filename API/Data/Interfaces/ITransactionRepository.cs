using API.DTOs;
using API.Models;

namespace API.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionDto>> GetTransactionList(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize);
        Task<List<TransactionModel>> InsertTransactions(List<TransactionModel> transactions);
    }
}
