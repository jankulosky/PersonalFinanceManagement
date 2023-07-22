using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> ImportTransactionsAsync(IFormFile csv);
        Task<PagedList<TransactionDto>> GetListAsync(FileParams fileParams);
        Task<TransactionDto> CategorizeTransactionAsync(int transactionId, CategorizeTransactionDto catCode);
        Task<List<AnalyticsDto>> GetTransactionAnalyticsAsync(AnalyticsParams analyticsParams);
        Task<TransactionDto> SplitTransactionAsync(int transactionId, TransactionSplitDto splits);
    }
}
