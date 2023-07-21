using API.Enumerations;

namespace API.Helpers
{
    public class AnalyticsParams
    {
        public string? CatCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Direction? Direction { get; set; }
    }
}
