using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TransactionModel> Transactions => Set<TransactionModel>();
        public DbSet<TransactionSplit> TransactionSplits => Set<TransactionSplit>();
        public DbSet<CategoryModel> Categories => Set<CategoryModel>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
