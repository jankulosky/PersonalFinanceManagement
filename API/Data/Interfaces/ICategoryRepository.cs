using API.DTOs;
using API.Helpers;

namespace API.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Response> ImportCategoriesFromFile(IFormFile csv);
        Task<List<CategoryDto>> GetCategoryList(CategoryParams categoryParams);
    }
}
