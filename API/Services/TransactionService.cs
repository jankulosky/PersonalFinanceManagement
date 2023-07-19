using API.Data.Interfaces;
using API.DTOs;
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

        public async Task<List<TransactionDto>> GetListAsync(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize)
        {
            return await _transactionRepository.GetTransactionList(transactionKind, startDate, endDate, page, pageSize);
        }

        public async Task<List<Transaction>> ImportTransactionsAsync(IFormFile csv)
        {
            return await _transactionRepository.ImportTransactionsFromFile(csv);
        }
    }
}
