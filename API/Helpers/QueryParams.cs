﻿using API.Enumerations;

namespace API.Helpers
{
    public class QueryParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public TransactionKind? TransactionKind { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.asc;
    }
}
