using API.Data.Interfaces;
using API.Models;
using API.Services.Interfaces;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> ImportCategoriesAsync(IFormFile csv)
        {
            return await _categoryRepository.ImportCategoriesFromFile(csv);
        }
    }
}
