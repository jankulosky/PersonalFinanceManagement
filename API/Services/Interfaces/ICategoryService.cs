using API.DTOs;
using API.Helpers;

namespace API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Response> ImportCategoriesAsync(IFormFile csv);
        Task<List<CategoryDto>> GetCategoryListAsync(CategoryParams categoryParams);
    }
}
