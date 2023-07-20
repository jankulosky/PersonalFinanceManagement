using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class TransactionSplit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public string CatCode { get; set; }
        public Category Category { get; set; }
        public Transaction Transaction { get; set; }
    }
}
