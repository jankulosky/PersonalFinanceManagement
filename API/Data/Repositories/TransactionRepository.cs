using API.Data.Interfaces;
using API.DTOs;
using API.Mappings;
using API.Models;
using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Transaction>> ImportTransactionsFromFile(IFormFile csv)
        {
            using var streamReader = new StreamReader(csv.OpenReadStream());
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap<TransactionMapper>();

            List<Transaction> transactions = csvReader.GetRecords<Transaction>().ToList();

            transactions = transactions.Where(t => t.Amount <= 0 && t.Id != null && !string.IsNullOrEmpty(t.Kind) && !string.IsNullOrEmpty(t.Direction))
                .ToList();

            foreach (var transaction in transactions)
            {
                if (transaction.Date.Kind == DateTimeKind.Unspecified)
                {
                    transaction.Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);
                }
            }

            return await InsertTransactions(transactions);
        }

        public async Task<List<TransactionDto>> GetTransactionList(string transactionKind, DateTime? startDate, DateTime? endDate, int? page, int? pageSize)
        {
            var transactions = _context.Transactions.Where(t => t.Kind == transactionKind);

            if (startDate != null)
            {
                transactions = transactions.Where(t => t.Date >= startDate);
            }

            if (endDate != null)
            {
                transactions = transactions.Where(t => t.Date <= endDate);
            }

            transactions = transactions.Skip((page ?? 0) * (pageSize ?? 25)).Take(pageSize ?? 25);

            var result = await transactions
                .Select(t => _mapper.Map<TransactionDto>(t))
                .ToListAsync();

            return result;
        }

        public async Task<List<Transaction>> InsertTransactions(List<Transaction> transactions)
        {
            var DbTransaction = _context.Database.BeginTransaction();

            await _context.Transactions.AddRangeAsync(transactions);

            await _context.SaveChangesAsync();

            await DbTransaction.CommitAsync();

            return transactions;
        }
    }
}
