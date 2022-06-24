using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
        //public string Password { get; set; }
        public int SavingsBalance { get; set; }
        public int CheckingBalance { get; set; }
        public int LoanBalance { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
