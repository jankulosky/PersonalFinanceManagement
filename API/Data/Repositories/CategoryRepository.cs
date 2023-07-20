using API.Data.Interfaces;
using API.Mappings;
using API.Models;
using CsvHelper;
using System.Globalization;

namespace API.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByCode(string code)
        {
            var result = await _context.Categories.FindAsync(code);

            if (result == null) return null;

            return result;
        }

        public async Task<List<Category>> ImportCategoriesFromFile(IFormFile csv)
        {
            using var streamReader = new StreamReader(csv.OpenReadStream());
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Context.RegisterClassMap<CategoryMapper>();

            var categories = csvReader.GetRecords<Category>().ToList();

            foreach (var category in categories)
            {
                var dbCategory = await _context.Categories.FindAsync(category.Code);

                if (dbCategory == null)
                {
                    _context.Categories.Add(category);
                }
                else
                {
                    dbCategory.ParentCode = category.ParentCode;
                    dbCategory.Name = category.Name;
                }
            }

            await _context.SaveChangesAsync();

            return categories;
        }
    }
}
