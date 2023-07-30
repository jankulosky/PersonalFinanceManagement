using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Services.Interfaces;

namespace API.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ITransactionRepository _transactionRepository;

        public AnalyticsService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<AnalyticsListDto> GetTransactionAnalyticsAsync(AnalyticsParams analyticsParams)
        {
            var transactions = await _transactionRepository.GetTransactionAnalytics(analyticsParams);

            var groups = transactions
                .GroupBy(t => t.CatCode);

            var spendingAnalyticsList = new AnalyticsListDto();

            foreach (var group in groups)
            {
                var analytic = new AnalyticsDto
                {
                    CatCode = group.Key,
                    Amount = Math.Round(group.Sum(t => t.Amount)),
                    Count = group.Count(),
                };
                spendingAnalyticsList.Groups.Add(analytic);
            }

            return spendingAnalyticsList;
        }
    }
}
