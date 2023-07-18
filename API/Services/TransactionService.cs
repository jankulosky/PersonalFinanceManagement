using API.Data.Repositories;
using API.DTOs;
using API.Mappings;
using API.Models;
using API.Services.Interfaces;
using CsvHelper;
using System.Globalization;

namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionDto>> GetListAsync(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize)
        {
            return await _transactionRepository.GetTransactionList(transactionKind, startDate, endDate, page, pageSize);
        }

        public async Task<List<TransactionModel>> ImportTransactionsAsync(IFormFile csv)
        {
            using (var streamReader = new StreamReader(csv.OpenReadStream()))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<TransactionMapper>();

                    List<TransactionModel> transactions = csvReader.GetRecords<TransactionModel>().ToList();

                    return transactions;
                }
            }
        }
    }
}
