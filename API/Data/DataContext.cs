using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionSplit> TransactionSplits => Set<TransactionSplit>();
        public DbSet<Category> Categories => Set<Category>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
