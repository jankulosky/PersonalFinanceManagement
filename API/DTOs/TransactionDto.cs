namespace API.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public string Direction { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public int? MCC { get; set; }
        public string Kind { get; set; }
        public CategoryDto? CategoryDto { get; set; }
        public List<SplitsDto> SplitsDto { get; set; }
    }
}
