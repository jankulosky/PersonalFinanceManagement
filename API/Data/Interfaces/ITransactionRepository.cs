using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<PagedList<TransactionDto>> GetTransactionList(TransactionParams fileParams);
        Task<List<Transaction>> InsertTransactions(List<Transaction> transactions);
        Task<TransactionResponse> CategorizeSingleTransaction(int transactionId, CategorizeTransactionDto catCode);
        Task<List<Transaction>> GetTransactionAnalytics(AnalyticsParams analyticsParams);
        Task<Transaction> GetTransactionWithSplits(int transactionId);
        Task<TransactionResponse> SplitTransaction(int transactionId, TransactionSplitDto splits);
        Task<Response> AutoCategorize();
    }
}
