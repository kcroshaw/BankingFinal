using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Banking.Interfaces;
using Banking.Data;

namespace Banking.Pages.Home
{
    public class LoanTransactionHistoryModel : PageModel
    {
        public List<Banking.Models.Transaction> Tran;
        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public Models.ApplicationUser ApplicationUser { get; set; }

        public LoanTransactionHistoryModel(ApplicationDBContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            Tran = _context.Transaction.ToList();
            var userName = User.Identity.Name;
            //getting the current application user and setting their balances to display on the dashboard page
            ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);

        }
    }
}
