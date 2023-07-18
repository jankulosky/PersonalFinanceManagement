using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] string transactionKind, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var result = await _transactionService.GetListAsync(transactionKind, startDate, endDate, page, pageSize);

            if (result == null) return NotFound("");

            return Ok(result);
        }

        [HttpPost("import")]
        public async Task<List<TransactionModel>> ImportTransactions(IFormFile csv)
        {
            return await _transactionService.ImportTransactionsAsync(csv);
        }
    }
}
