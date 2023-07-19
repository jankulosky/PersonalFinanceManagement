using API.Models;

namespace API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> ImportCategoriesAsync(IFormFile csv);
    }
}
