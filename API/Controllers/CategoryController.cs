using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("import")]
        public async Task<ActionResult<List<Category>>> ImportCategories(IFormFile csv)
        {
            try
            {
                var categories = await _categoryService.ImportCategoriesAsync(csv);

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
