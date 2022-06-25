using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banking.Pages.Home
{
    public class CheckingTransactionHistoryModel : PageModel
    {
        public int v;

        public void OnGet()
        {
            v = 3;
        }
    }
}
