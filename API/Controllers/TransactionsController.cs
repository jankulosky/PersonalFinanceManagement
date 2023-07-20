using API.Extensions;
using API.Helpers;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionsController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetTransactions([FromQuery] QueryParams queryParams)
        {
            try
            {
                var transactions = await _transactionService.GetListAsync(queryParams);

                if (transactions == null) return NotFound();

                Response.AddPaginationHeader(transactions.CurrentPage, transactions.PageSize, transactions.TotalCount, transactions.TotalPages);

                return Ok(transactions);
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

        [HttpPost("{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] int id, [FromBody] string categoryCode)
        {
            try
            {
                var result = await _transactionService.CategorizeTransactionAsync(id, categoryCode);

                if (result == null)
                {
                    return NotFound("Transaction or category not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
