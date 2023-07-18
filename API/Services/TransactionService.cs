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

                    transactions = transactions.Where(t =>
                    {
                        if (t.Amount < 0)
                        {
                            return false;
                        }
                        else if (t.Id == null)
                        {
                            return false;
                        }
                        else if (t.Kind == null)
                        {
                            return false;
                        }
                        else if (t.Kind == null)
                        {
                            return false;
                        }
                        else if (t.Direction == null)
                        {
                            return false;
                        }
                        return true;
                    }).ToList();

                    foreach (var transaction in transactions)
                    {
                        if (transaction.Date.Kind == DateTimeKind.Unspecified)
                        {
                            transaction.Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);
                        }
                    }

                    return await _transactionRepository.InsertTransactions(transactions);
                }
            }
        }
    }
}
