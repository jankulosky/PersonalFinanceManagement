using API.DTOs;

namespace API.Helpers
{
    public class TransactionResponse
    {
        public TransactionDto Transaction { get; set; }
        public List<ErrorDetails> Errors { get; set; }
    }
}
