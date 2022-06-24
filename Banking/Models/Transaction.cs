using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{

    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Required]
        public string UserID { get; set; }

        public string Account { get; set; }

        public DateTime TransactionDate { get; set; }

        public int TransactionAmount { get; set; }
    }
}
