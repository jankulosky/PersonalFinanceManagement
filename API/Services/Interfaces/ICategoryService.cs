using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> ImportCategoriesAsync(IFormFile csv);
        Task<PagedList<CategoryDto>> GetCategoryListAsync(PaginationParams paginationParams);
    }
}
