using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banking.Pages.Home
{
    public class CheckingTransactionHistoryModel : PageModel
    {
        private Data.ApplicationDBContext db;
        public List<Banking.Models.Transaction> Tran;

        public CheckingTransactionHistoryModel(Data.ApplicationDBContext _db)
        {
            db = _db;
        }

        public void OnGet()
        {
            Tran = db.Transaction.ToList();
        }
    }
}
