using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public TransactionConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CatCode);
            builder
                .Property(t => t.Currency)
                .HasMaxLength(3);
            builder.HasMany(t => t.Splits);
            builder
                .Property(t => t.Direction)
                .HasConversion<string>();
            builder
                .Property(t => t.Kind)
                .HasConversion<string>();
        }
    }
}
