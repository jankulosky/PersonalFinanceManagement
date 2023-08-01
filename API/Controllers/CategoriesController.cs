using API.DTOs;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategories(IFormFile csv)
        {
            try
            {
                var categories = await _categoryService.ImportCategoriesAsync(csv);

                if (categories.Error != null && categories.Error.Any())
                {
                    return BadRequest(new { errors = categories.Error });
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] CategoryParams categoryParams)
        {
            try
            {
                var categories = await _categoryService.GetCategoryListAsync(categoryParams);

                if (categories == null) return NotFound();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
