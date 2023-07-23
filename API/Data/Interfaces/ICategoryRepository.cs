using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> ImportCategoriesFromFile(IFormFile csv);
        Task<PagedList<CategoryDto>> GetCategoryList(PaginationParams paginationParams);
    }
}
