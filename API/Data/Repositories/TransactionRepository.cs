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

        public async Task<PagedList<TransactionDto>> GetTransactionList(QueryParams queryParams)
        {
            var query = _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Splits)
                .AsQueryable();

            if (queryParams.TransactionKind.HasValue)
            {
                query = query.Where(t => t.Kind == queryParams.TransactionKind.Value);
            }

            if (queryParams.StartDate.HasValue)
            {
                query = query.Where(t => t.Date >= queryParams.StartDate.Value);
            }

            if (queryParams.EndDate.HasValue)
            {
                query = query.Where(t => t.Date <= queryParams.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(queryParams.TransactionKind.ToString()))
            {
                if (Enum.TryParse<TransactionKind>(queryParams.TransactionKind.ToString(), true, out var transactionKind))
                {
                    query = query.Where(t => t.Kind == transactionKind);
                }
                else
                {
                    throw new ArgumentException("Invalid value for transaction kind.");
                }
            }

            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "date":
                        if (queryParams.SortOrder == SortOrder.desc)
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

            return await PagedList<TransactionDto>.CreateAsync(pagedList, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<List<Transaction>> InsertTransactions(List<Transaction> transactions)
        {
            var DbTransaction = _context.Database.BeginTransaction();

            await _context.Transactions.AddRangeAsync(transactions);

            await _context.SaveChangesAsync();

            await DbTransaction.CommitAsync();

            return transactions;
        }

        public async Task<TransactionDto> CategorizeSingleTransaction(int id, string catCode)
        {
            var dbTransaction = _context.Database.BeginTransaction();

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                throw new ArgumentException($"Transaction with ID {id} not found.");
            }

            var category = await _context.Categories.FindAsync(catCode);

            if (category == null)
            {
                throw new ArgumentException($"Category with code '{catCode}' not found.");
            }

            transaction.CatCode = catCode;
            transaction.Category = category;

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return _mapper.Map<TransactionDto>(transaction);

        }
    }
}
