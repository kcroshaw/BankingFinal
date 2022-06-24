using Banking.Data;
using Banking.Interfaces;
using Banking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Banking.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public int savingsBalance = 0;
        public int checkingBalance = 0;
        public int loanBalance = 0;
        public int transactionAmount = 0;



        public DashboardModel(ApplicationDBContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Transaction Transaction { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                //getting the current application user and setting their balances to display on the dashboard page
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);
                savingsBalance = ApplicationUser.SavingsBalance;
                checkingBalance = ApplicationUser.CheckingBalance;
                loanBalance = ApplicationUser.LoanBalance;

                // We will also need to populate a list of all transactions here that are specific
                // to the applicationuser

                return Page();
            }
            else
                return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        public void CreateTransaction(string transactionType, string account, int amount)
        {
            Transaction.TransactionDate = DateTime.Now;
            Transaction.TransactionType = transactionType;
            Transaction.Account = account;
            Transaction.TransactionAmount = ConvertFromPennies(amount);
            Transaction.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        }


        public async Task<IActionResult> OnPostTransfer()
        {
            // this is where we will need to handle the changing of the balances and adding to
            // a list of transactions
            //capture form data
            string transType = "Transfer";
            var accountFrom = Request.Form["transferFrom"];
            var accountTo = Request.Form["transTo"];

            //convert amount to pennies and store in positive and negative variables
            int posAmount = ConvertToPennies(Request.Form["transferAmt"]);
            int negAmount = posAmount * (-1); // doing this so a negative value will reflect in the transaction record


            // this is where we will need to handle the changing of the balances and adding to
            // a list of transactions
            //this is the original test code Kolby made************
            var userName = User.Identity.Name;
            ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);
            ApplicationUser.SavingsBalance = 10;
            int x = ApplicationUser.SavingsBalance;
            Transaction.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //ApplicationUser.SavingsBalance = 10;
            //int x = ApplicationUser.SavingsBalance;
            //Transaction.TransactionType = TransactionType.Transfer;
            //Transaction.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Transaction.AccountID = 11;
            //Transaction.Account = "savings";
            //Transaction.TransactionDate = DateTime.Now;
            _context.Transaction.Add(Transaction);
            await _context.SaveChangesAsync();
            //CreateTransaction(transType, )
            //************************************

            //figure out the FROM account stuff first
            if (accountFrom == "Savings" || accountFrom == "Checking")//if the account FROM is checking or savings use negative amount and create transaction record
            {
                CreateTransaction(transType, accountFrom, negAmount);

                if (accountFrom == "Savings")
                {
                    //adjust savings balance
                    ApplicationUser.SavingsBalance += negAmount;
                }
                else if (accountFrom == "Checking")
                {
                    //adjust checking balance
                    ApplicationUser.CheckingBalance += negAmount;
                }

            }
            else if (accountFrom == "Loan")//we are going to add the amount to the loan
            {
                CreateTransaction(transType, accountFrom, posAmount);
                ApplicationUser.LoanBalance += posAmount;
            }

            //figure out the TO account stuff next
            if (accountTo == "Savings" || accountTo == "Checking")//if the account TO is checking or savings use positive amount and create transaction record
            {
                CreateTransaction(transType, accountFrom, posAmount);

                if (accountTo == "Savings")
                {
                    //adjust savings balance
                    ApplicationUser.SavingsBalance += posAmount;
                }
                else if (accountTo == "Checking")
                {
                    //adjust checking balance
                    ApplicationUser.CheckingBalance += posAmount;
                }

            }
            else if (accountTo == "Loan")//we are going to subtract the amount from the loan
            {
                CreateTransaction(transType, accountFrom, negAmount);
                ApplicationUser.LoanBalance += negAmount;
            }

            return RedirectToPage("./Index");
        }

        //If Deposit/Withdraw button is clicked
        public async Task<IActionResult> OnPostDepositWithdraw()
        {

            //TODO: create logic for deposit/withdraw modal

            return RedirectToPage("./Index");
        }
    }
}
