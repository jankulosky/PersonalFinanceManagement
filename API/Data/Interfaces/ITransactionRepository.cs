using API.DTOs;
using API.Models;

namespace API.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> ImportTransactionsFromFile(IFormFile csv);
        Task<List<TransactionDto>> GetTransactionList(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize);
        Task<List<Transaction>> InsertTransactions(List<Transaction> transactions);
    }
}
