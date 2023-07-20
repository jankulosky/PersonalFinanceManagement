using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Enumerations;

namespace API.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public Direction Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        [ForeignKey("MccCodes")]
        public int? MCC { get; set; }
        public TransactionKind Kind { get; set; }
        [ForeignKey("Category")]
        public string? CatCode { get; set; }
        public Category? Category { get; set; }
        public List<TransactionSplit> Splits { get; set; }
    }
}
