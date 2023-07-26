using API.DTOs;
using API.Extensions;
using API.Helpers;
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
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions([FromQuery] FileParams fileParams)
        {
            try
            {
                var transactions = await _transactionService.GetListAsync(fileParams);

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
        public async Task<IActionResult> ImportTransactions(IFormFile csv)
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
        public async Task<IActionResult> CategorizeTransaction([FromRoute] int id, [FromBody] CategorizeTransactionDto catCode)
        {
            try
            {
                var result = await _transactionService.CategorizeTransactionAsync(id, catCode);

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

        [HttpGet("spending-analytics")]
        public async Task<ActionResult<IEnumerable<AnalyticsDto>>> GetSpendingAnalytics([FromQuery] AnalyticsParams analyticsParams)
        {
            try
            {
                var result = await _transactionService.GetTransactionAnalyticsAsync(analyticsParams);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromRoute] int id, [FromBody] TransactionSplitDto splits)
        {
            try
            {
                var result = await _transactionService.SplitTransactionAsync(id, splits);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("auto-categorize")]
        public async Task<IActionResult> AutoCategorizeTransactions()
        {
            try
            {
                var result = await _transactionService.AutoCategorizeAsync();

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
