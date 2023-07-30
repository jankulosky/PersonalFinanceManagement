using API.DTOs;
using API.Helpers;

namespace API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Response> ImportCategoriesAsync(IFormFile csv);
        Task<PagedList<CategoryDto>> GetCategoryListAsync(CategoryParams categoryParams);
    }
}
