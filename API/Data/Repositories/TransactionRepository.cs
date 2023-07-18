using API.Data.Interfaces;
using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
    }
}
