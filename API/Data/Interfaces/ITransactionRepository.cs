using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> ImportTransactionsFromFile(IFormFile csv);
        Task<PagedList<TransactionDto>> GetTransactionList(QueryParams queryParams);
        Task<List<Transaction>> InsertTransactions(List<Transaction> transactions);
        Task<TransactionDto> CategorizeSingleTransaction(int id, string catCode);
    }
}
