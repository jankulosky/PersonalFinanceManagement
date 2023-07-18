using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class TransactionSplit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TransactionModel Transaction { get; set; }
        public CategoryModel Category { get; set; }
        public double Amount { get; set; }
    }
}
