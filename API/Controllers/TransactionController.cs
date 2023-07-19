using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetTransactions([FromQuery] string transactionKind, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            try
            {
                var result = await _transactionService.GetListAsync(transactionKind, startDate, endDate, page, pageSize);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }

        }

        [HttpPost("import")]
        public async Task<ActionResult<List<Transaction>>> ImportTransactions(IFormFile csv)
        {
            try
            {
                var transactions = await _transactionService.ImportTransactionsAsync(csv);

                if (transactions == null) return NotFound();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
