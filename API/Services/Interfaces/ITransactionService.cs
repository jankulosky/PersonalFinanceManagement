using API.DTOs;
using API.Models;

namespace API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> ImportTransactionsAsync(IFormFile csv);
        Task<List<TransactionDto>> GetListAsync(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize);
    }
}
