using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Can't have more than 2 decimal places")]
        [Precision(18,2)]
        public decimal TransactionAmount { get; set; }
    }
}
