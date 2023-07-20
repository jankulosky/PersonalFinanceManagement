using API.Models;

namespace API.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> ImportCategoriesFromFile(IFormFile csv);
        Task<Category> GetCategoryByCode(string code);
    }
}
