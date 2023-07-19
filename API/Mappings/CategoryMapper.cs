using API.Models;
using CsvHelper.Configuration;

namespace API.Mappings
{
    public class CategoryMapper : ClassMap<Category>
    {
        public CategoryMapper()
        {
            Map(m => m.Code).Name("code");
            Map(m => m.ParentCode).Name("parent-code");
            Map(m => m.Name).Name("name");
        }
    }
}
