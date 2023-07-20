using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasKey(c => c.Code);
            builder
                .HasMany(c => c.Transactions);
            builder
                .Property(c => c.ParentCode)
                .IsRequired(false);
        }
    }
}
