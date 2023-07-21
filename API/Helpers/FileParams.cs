using API.Enumerations;

namespace API.Helpers
{
    public class FileParams : PaginationParams
    {
        public TransactionKind? TransactionKind { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.asc;
    }
}
