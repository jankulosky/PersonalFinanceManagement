using API.DTOs;
using API.Helpers;

namespace API.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsListDto> GetTransactionAnalyticsAsync(AnalyticsParams analyticsParams);
    }
}
