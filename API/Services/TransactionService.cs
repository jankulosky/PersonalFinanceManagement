using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Models;
using API.Services.Interfaces;

namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDto> CategorizeTransactionAsync(int id, string catCode)
        {
            return await _transactionRepository.CategorizeSingleTransaction(id, catCode);
        }

        public async Task<PagedList<TransactionDto>> GetListAsync(QueryParams queryParams)
        {
            return await _transactionRepository.GetTransactionList(queryParams);
        }

        public async Task<List<Transaction>> ImportTransactionsAsync(IFormFile csv)
        {
            return await _transactionRepository.ImportTransactionsFromFile(csv);
        }
    }
}
