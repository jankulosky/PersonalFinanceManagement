using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class TransactionSplitConfiguration : IEntityTypeConfiguration<TransactionSplit>
    {
        public TransactionSplitConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<TransactionSplit> builder)
        {
            builder.HasKey(ts => ts.Id);
            builder
                .HasOne(ts => ts.Transaction)
                .WithMany(t => t.Splits)
                .HasForeignKey(ts => ts.TransactionId);
        }
    }
}
