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
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions([FromQuery] TransactionParams transactionParams)
        {
            try
            {
                var transactions = await _transactionService.GetListAsync(transactionParams);

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

                if (transactions.Error != null && transactions.Error.Any())
                {
                    return BadRequest(new { errors = transactions.Error });
                }

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

                if (result.Errors != null && result.Errors.Any())
                {
                    return BadRequest(new { errors = result.Errors });
                }

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

                if (result.Errors != null && result.Errors.Any())
                {
                    return BadRequest(new { errors = result.Errors });
                }

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
