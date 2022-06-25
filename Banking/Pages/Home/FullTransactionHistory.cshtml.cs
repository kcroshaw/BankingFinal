using Banking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banking.Pages.Home
{
    public class FullTransactionHistoryModel : PageModel {
        private Data.ApplicationDBContext db;

        public List<Banking.Models.Transaction> Tran;
        [BindProperty]
        public Transaction TransactionA { get; set; }

        public FullTransactionHistoryModel(Data.ApplicationDBContext _db)
        {
            db = _db;
        }

        public void OnGet()
        {
            Tran = db.Transaction.ToList();

        }
    }
}
