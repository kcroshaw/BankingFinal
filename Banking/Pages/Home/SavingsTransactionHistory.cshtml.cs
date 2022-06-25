using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banking.Pages.Home
{
    public class SavingsTransactionHistoryModel : PageModel
    {
        private Data.ApplicationDBContext db;
        public List<Banking.Models.Transaction> Tran;

        public SavingsTransactionHistoryModel(Data.ApplicationDBContext _db)
        {
            db = _db;
        }

        public void OnGet()
        {
            Tran = db.Transaction.ToList();
        }
    }
}
