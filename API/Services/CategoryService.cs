using API.Data.Interfaces;
using API.DTOs;
using API.Helpers;
using API.Services.Interfaces;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> GetCategoryListAsync(CategoryParams categoryParams)
        {
            return await _unitOfWork.CategoryRepository.GetCategoryList(categoryParams);
        }

        public async Task<Response> ImportCategoriesAsync(IFormFile csv)
        {
            return await _unitOfWork.CategoryRepository.ImportCategoriesFromFile(csv);
        }
    }
}
