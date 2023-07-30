using API.DTOs;
using API.Helpers;

namespace API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Response> ImportTransactionsAsync(IFormFile csv);
        Task<PagedList<TransactionDto>> GetListAsync(TransactionParams fileParams);
        Task<TransactionResponse> CategorizeTransactionAsync(int transactionId, CategorizeTransactionDto catCode);
        Task<TransactionResponse> SplitTransactionAsync(int transactionId, TransactionSplitDto splits);
        Task<Response> AutoCategorizeAsync();
    }
}
