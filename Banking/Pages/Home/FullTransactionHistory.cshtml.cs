using Banking.Data;
using Banking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Banking.Interfaces;

namespace Banking.Pages.Home
{
    public class FullTransactionHistoryModel : PageModel {

        private Data.ApplicationDBContext db;
        public List<Banking.Models.Transaction> Tran;
        public List<Banking.Models.Transaction> Trans;

        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationUser ApplicationUser { get; set; }
        public Transaction Transaction { get; set; }

        // public FullTransactionHistoryModel(Data.ApplicationDBContext _db)
        //{
        // db = _db;
        //}
        public FullTransactionHistoryModel(ApplicationDBContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public string p;

        public void OnGet()
        {
            Trans = _context.Transaction.ToList();

            var userName = User.Identity.Name;
            //getting the current application user and setting their balances to display on the dashboard page
            ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);
           
            //foreach(var t in Trans)
            //{
           //     if(t.UserID == ApplicationUser.Id) { Tran.Add(t); }
            //}

            Tran = Trans;

        }
    }
}
