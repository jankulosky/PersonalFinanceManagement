using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
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

        public async Task<PagedList<CategoryDto>> GetCategoryListAsync(CategoryParams categoryParams)
        {
            return await _categoryRepository.GetCategoryList(categoryParams);
        }

        public async Task<Response> ImportCategoriesAsync(IFormFile csv)
        {
            return await _categoryRepository.ImportCategoriesFromFile(csv);
        }
    }
}
