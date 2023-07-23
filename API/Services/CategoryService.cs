using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
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

        public async Task<PagedList<CategoryDto>> GetCategoryListAsync(PaginationParams paginationParams)
        {
            return await _categoryRepository.GetCategoryList(paginationParams);
        }

        public async Task<List<Category>> ImportCategoriesAsync(IFormFile csv)
        {
            return await _categoryRepository.ImportCategoriesFromFile(csv);
        }
    }
}
