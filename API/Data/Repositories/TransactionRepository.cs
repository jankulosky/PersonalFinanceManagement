using API.Data.Interfaces;
using API.DTOs;
using API.Enumerations;
using API.Helpers;
using API.Mappings;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var streamReader = new StreamReader(csv.OpenReadStream());
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap<TransactionMapper>();

            List<Transaction> transactions = csvReader.GetRecords<Transaction>().ToList();

            return await InsertTransactions(transactions);
        }

        public async Task<PagedList<TransactionDto>> GetTransactionList(FileParams fileParams)
        {
            var query = _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Splits)
                .AsQueryable();

            if (fileParams.TransactionKind.HasValue)
            {
                query = query.Where(t => t.Kind == fileParams.TransactionKind.Value);
            }

            if (fileParams.StartDate.HasValue)
            {
                query = query.Where(t => t.Date >= fileParams.StartDate.Value);
            }

            if (fileParams.EndDate.HasValue)
            {
                query = query.Where(t => t.Date <= fileParams.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(fileParams.TransactionKind.ToString()))
            {
                if (Enum.TryParse<TransactionKind>(fileParams.TransactionKind.ToString(), true, out var transactionKind))
                {
                    query = query.Where(t => t.Kind == transactionKind);
                }
                else
                {
                    throw new ArgumentException("Invalid value for transaction kind.");
                }
            }

            if (!string.IsNullOrEmpty(fileParams.SortBy))
            {
                switch (fileParams.SortBy.ToLower())
                {
                    case "date":
                        if (fileParams.SortOrder == SortOrder.desc)
                        {
                            query = query.OrderByDescending(t => t.Date);
                        }
                        else
                        {
                            query = query.OrderBy(t => t.Date);
                        }
                        break;
                    default:
                        query = query.OrderBy(t => t.Date);
                        break;
                }
            }

            var pagedList = query
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<TransactionDto>.CreateAsync(pagedList, fileParams.PageNumber, fileParams.PageSize);
        }

        public async Task<List<Transaction>> InsertTransactions(List<Transaction> transactions)
        {
            var DbTransaction = _context.Database.BeginTransaction();

            await _context.Transactions.AddRangeAsync(transactions);

            await _context.SaveChangesAsync();

            await DbTransaction.CommitAsync();

            return transactions;
        }

        public async Task<TransactionDto> CategorizeSingleTransaction(int transactionId, CategorizeTransactionDto catCode)
        {
            var dbTransaction = _context.Database.BeginTransaction();

            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                throw new ArgumentException($"Transaction with ID {transactionId} not found.");
            }

            var category = await _context.Categories.FindAsync(catCode.CatCode);

            if (category == null)
            {
                throw new ArgumentException($"Category with code '{catCode}' not found.");
            }

            transaction.CatCode = catCode.CatCode;
            transaction.Category = category;

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return _mapper.Map<TransactionDto>(transaction);

        }

        public async Task<List<AnalyticsDto>> GetTransactionAnalytics(AnalyticsParams analyticsParams)
        {
            var query = _context.Transactions
                .Where(t => t.CatCode != null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(analyticsParams.CatCode))
            {
                query = query.Where(t => t.CatCode == analyticsParams.CatCode);
            }

            if (analyticsParams.StartDate != null)
            {
                query = query.Where(t => t.Date >= analyticsParams.StartDate);
            }

            if (analyticsParams.EndDate != null)
            {
                query = query.Where(t => t.Date <= analyticsParams.EndDate);
            }

            if (analyticsParams.Direction != null)
            {
                query = query.Where(t => t.Direction == analyticsParams.Direction);
            }

            var spendingAnalytics = await query
                .GroupBy(t => new { t.CatCode, t.Category.Name })
                .Select(g => new AnalyticsDto
                {
                    CatCode = g.Key.CatCode,
                    CategoryName = g.Key.Name,
                    TotalSpending = g.Sum(t => t.Amount)
                })
                .ToListAsync();

            return spendingAnalytics;
        }

        public async Task<Transaction> GetTransactionWithSplits(int transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Splits)
                .SingleOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<TransactionDto> SplitTransaction(int transactionId, TransactionSplitDto splitsDto)
        {
            var dbTransaction = _context.Database.BeginTransaction();

            var transaction = await GetTransactionWithSplits(transactionId);

            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found.");
            }

            if (transaction.Splits == null)
            {
                transaction.Splits = new List<TransactionSplit>();
            }

            _context.TransactionSplits.RemoveRange(transaction.Splits);

            var splits = splitsDto.SplitsDto.Select(dto => new TransactionSplit
            {
                TransactionId = transactionId,
                Amount = dto.Amount,
                CatCode = dto.CatCode
            }).ToList();

            await _context.TransactionSplits.AddRangeAsync(splits);

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
