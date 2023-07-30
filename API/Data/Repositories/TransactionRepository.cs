using API.Data.Interfaces;
using API.DTOs;
using API.Enumerations;
using API.Helpers;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

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

        public async Task<PagedList<TransactionDto>> GetTransactionList(TransactionParams transactionParams)
        {
            var query = _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Splits)
                .AsQueryable();

            if (transactionParams.TransactionKind.HasValue)
            {
                query = query.Where(t => t.Kind == transactionParams.TransactionKind.Value);
            }

            if (transactionParams.StartDate.HasValue)
            {
                query = query.Where(t => t.Date >= transactionParams.StartDate.Value);
            }

            if (transactionParams.EndDate.HasValue)
            {
                query = query.Where(t => t.Date <= transactionParams.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(transactionParams.TransactionKind.ToString()))
            {
                if (Enum.TryParse<TransactionKind>(transactionParams.TransactionKind.ToString(), true, out var transactionKind))
                {
                    query = query.Where(t => t.Kind == transactionKind);
                }
                else
                {
                    throw new ArgumentException("Invalid value for transaction kind.");
                }
            }

            if (!string.IsNullOrEmpty(transactionParams.SortBy))
            {
                switch (transactionParams.SortBy.ToLower())
                {
                    case "date":
                        if (transactionParams.SortOrder == SortOrder.desc)
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

            return await PagedList<TransactionDto>.CreateAsync(pagedList, transactionParams.PageNumber, transactionParams.PageSize);
        }

        public async Task<List<Transaction>> InsertTransactions(List<Transaction> transactions)
        {
            var DbTransaction = _context.Database.BeginTransaction();

            foreach (var transaction in transactions)
            {
                var existingTransaction = await _context.Transactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == transaction.Id);

                if (existingTransaction == null)
                {
                    await _context.Transactions.AddRangeAsync(transaction);
                }
            }

            await _context.SaveChangesAsync();

            await DbTransaction.CommitAsync();

            return transactions;
        }

        public async Task<TransactionResponse> CategorizeSingleTransaction(int transactionId, CategorizeTransactionDto catCode)
        {
            var dbTransaction = _context.Database.BeginTransaction();

            var transaction = await _context.Transactions.FindAsync(transactionId);

            var category = await _context.Categories.FindAsync(catCode.CatCode);

            if (category == null)
            {
                return new TransactionResponse
                {
                    Errors = new List<ErrorDetails>
                    {
                        new ErrorDetails
                        {
                            Tag = "category",
                            Error = "not-found",
                            Message = $"Category with code '{catCode.CatCode}' not found."
                        }
                    }
                };
            }

            transaction.CatCode = catCode.CatCode;
            transaction.Category = category;

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return new TransactionResponse
            {
                Transaction = _mapper.Map<TransactionDto>(transaction)
            };
        }

        public async Task<List<Transaction>> GetTransactionAnalytics(AnalyticsParams analyticsParams)
        {
            var query = _context.Transactions
                .Where(t => t.CatCode != null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(analyticsParams.CatCode))
            {
                query = query.Where(t => analyticsParams.CatCode.Contains(t.CatCode));
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

            return await query.ToListAsync();
        }

        public async Task<Transaction> GetTransactionWithSplits(int transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Splits)
                .SingleOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<TransactionResponse> SplitTransaction(int transactionId, TransactionSplitDto splitsDto)
        {
            var dbTransaction = _context.Database.BeginTransaction();

            var transaction = await GetTransactionWithSplits(transactionId);

            if (transaction == null)
            {
                return new TransactionResponse
                {
                    Errors = new List<ErrorDetails>
                    {
                        new ErrorDetails
                        {
                            Tag = "transaction",
                            Error = "not-found",
                            Message = $"Transaction with ID {transactionId} not found."
                        }
                    }
                };
            }

            if (transaction.Splits.Any())
            {
                _context.TransactionSplits.RemoveRange(transaction.Splits);
            }

            var invalidCategories = splitsDto.Splits
                .Select(dto => dto.CatCode)
                .Except(_context.Categories.Select(c => c.Code))
                .ToList();

            if (invalidCategories.Any())
            {
                await dbTransaction.RollbackAsync();
                return new TransactionResponse
                {
                    Errors = invalidCategories.Select(categoryCode => new ErrorDetails
                    {
                        Tag = "category",
                        Error = "invalid",
                        Message = $"Invalid category code: {categoryCode}"
                    }).ToList()
                };
            }

            double totalSplitAmount = splitsDto.Splits.Sum(dto => dto.Amount);

            if (totalSplitAmount > transaction.Amount)
            {
                await dbTransaction.RollbackAsync();
                return new TransactionResponse
                {
                    Errors = new List<ErrorDetails>
                    {
                        new ErrorDetails
                        {
                            Tag = "amount",
                            Error = "max-length",
                            Message = "Total split amount cannot exceed the transaction amount."
                        }
                    }
                };
            }

            double splitAmountSum = splitsDto.Splits.Sum(dto => dto.Amount);
            if (Math.Abs(splitAmountSum - transaction.Amount) > 0.001)
            {
                await dbTransaction.RollbackAsync();
                return new TransactionResponse
                {
                    Errors = new List<ErrorDetails>
                    {
                        new ErrorDetails
                        {
                            Tag = "amount",
                            Error = "invalid-sum",
                            Message = "Sum of split amounts must be equal to the transaction amount."
                        }
                    }
                };
            }

            var splits = splitsDto.Splits.Select(dto => new TransactionSplit
            {
                TransactionId = transactionId,
                Amount = dto.Amount,
                CatCode = dto.CatCode
            }).ToList();

            await _context.TransactionSplits.AddRangeAsync(splits);

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return new TransactionResponse
            {
                Transaction = _mapper.Map<TransactionDto>(transaction)
            };
        }

        public async Task<Response> AutoCategorize()
        {
            var transactions = _context.Transactions
                .Include(t => t.Category)
                .ToList();

            List<RuleParams> rules = new();

            using (StreamReader sr = new("Data/rules.json"))
            {
                string json = await sr.ReadToEndAsync();
                rules = JsonConvert.DeserializeObject<List<RuleParams>>(json).ToList();
            }

            int categorizedCount = 0;

            foreach (var transaction in transactions)
            {
                if (!string.IsNullOrEmpty(transaction.CatCode))
                {
                    continue;
                }

                foreach (var rule in rules)
                {
                    if (rule.Mcc.Any(x => x == transaction.MCC) ||
                        rule.Keywords.Any(x => transaction.BeneficiaryName.ToLower().Contains(x.ToLower()) ||
                                               transaction.Description.ToLower().Contains(x.ToLower())))
                    {
                        transaction.CatCode = rule.CatCode;
                        categorizedCount++;
                        break;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new Response
            {
                Message = $"{categorizedCount} transactions were automatically categorized."
            };
        }
    }
}
