using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Mappings;
using API.Models;
using API.Services.Interfaces;
using CsvHelper;
using System.Globalization;

namespace API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Response> AutoCategorizeAsync()
        {
            return await _transactionRepository.AutoCategorize();
        }

        public async Task<TransactionResponse> CategorizeTransactionAsync(int transactionId, CategorizeTransactionDto catCode)
        {
            return await _transactionRepository.CategorizeSingleTransaction(transactionId, catCode);
        }

        public async Task<PagedList<TransactionDto>> GetListAsync(TransactionParams fileParams)
        {
            return await _transactionRepository.GetTransactionList(fileParams);
        }

        public async Task<Response> ImportTransactionsAsync(IFormFile csv)
        {
            try
            {
                var streamReader = new StreamReader(csv.OpenReadStream());
                var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

                csvReader.Context.RegisterClassMap<TransactionMapper>();

                List<Transaction> transactions = csvReader.GetRecords<Transaction>().ToList();

                await _transactionRepository.InsertTransactions(transactions);

                return new Response
                {
                    Message = "Transactions imported successfully."
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    Error = $"An error occured while reading file: '{csv.FileName}'. Please ensure that the file you imported is a valid CSV file and it contains the expected headers in the correct order."
                };
            }
        }

        public async Task<TransactionResponse> SplitTransactionAsync(int transactionId, TransactionSplitDto splits)
        {
            return await _transactionRepository.SplitTransaction(transactionId, splits);
        }
    }
}
