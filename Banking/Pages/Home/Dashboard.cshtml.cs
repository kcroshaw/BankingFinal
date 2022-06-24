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

        public decimal savingsBalance = 0.00M;
        public decimal checkingBalance = 0.00M;
        public decimal loanBalance = 0.00M;
        public decimal transactionAmount = 0.00M;

        public int ConvertToPennies(string amount)
        {
            decimal dollarAmt = Convert.ToDecimal(amount);

            int pennies = (int)(dollarAmt * 100);

            return pennies;
        }

        public decimal ConvertFromPennies(int pennies)
        {
            decimal dollarAmt = Convert.ToDecimal(pennies)/100;

            return dollarAmt;
        }

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
                savingsBalance = ConvertFromPennies(ApplicationUser.SavingsBalance);
                checkingBalance = ConvertFromPennies(ApplicationUser.CheckingBalance);
                loanBalance = ConvertFromPennies(ApplicationUser.LoanBalance);

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
            var accountTo = Request.Form["transferTo"];
            var userName = User.Identity.Name;
            ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);

            //convert amount to pennies
            int amtInPennies = ConvertToPennies(Request.Form["transferAmt"]);
  
            //figure out the FROM account stuff first
            if (accountFrom == "Savings" || accountFrom == "Checking")//if the account FROM is checking or savings use negative amount and create transaction record
            {
                if (accountFrom == "Savings")
                {
                    //adjust savings balance
                    ApplicationUser.SavingsBalance += amtInPennies * (-1);
                }
                else if (accountFrom == "Checking")
                {
                    //adjust checking balance
                    ApplicationUser.CheckingBalance += amtInPennies * (-1);
                }
                CreateTransaction(transType, accountFrom, amtInPennies * (-1));
                _context.Transaction.Add(Transaction);
            }
            else if (accountFrom == "Loan")//we are going to add the amount to the loan
            {
                ApplicationUser.LoanBalance += amtInPennies;
                CreateTransaction(transType, accountFrom, amtInPennies);
                _context.Transaction.Add(Transaction);
            }

            //figure out the TO account stuff next
            if (accountTo == "Savings" || accountTo == "Checking")//if the account TO is checking or savings use positive amount and create transaction record
            {
                if (accountTo == "Savings")
                {
                    //adjust savings balance
                    ApplicationUser.SavingsBalance += amtInPennies;
                }
                else if (accountTo == "Checking")
                {
                    //adjust checking balance
                    ApplicationUser.CheckingBalance += amtInPennies;
                }
                CreateTransaction(transType, accountFrom, amtInPennies);
                _context.Transaction.Add(Transaction);
            }
            else if (accountTo == "Loan")//we are going to subtract the amount from the loan
            {
                ApplicationUser.LoanBalance += amtInPennies * (-1);
                CreateTransaction(transType, accountFrom, amtInPennies * (-1));
                _context.Transaction.Add(Transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        //If Deposit/Withdraw button is clicked
        public async Task<IActionResult> OnPostDepositWithdraw()
        {
            //TODO: create logic for deposit/withdraw modal
            var userName = User.Identity.Name;
            ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.UserName == userName);

            var transType = Request.Form["transactionType"];
            var account = Request.Form["depWthAccount"];
            var amtInPennies = ConvertToPennies(Request.Form["depWthAmt"]);

            if (transType == "Deposit")
            {
                if(account == "Savings" || account  == "Checking")
                {
                    if(account == "Savings")
                    {
                        ApplicationUser.SavingsBalance += amtInPennies;
                    }
                    else if(account == "Checking")
                    {
                        ApplicationUser.CheckingBalance += amtInPennies;
                    }
                    
                    CreateTransaction(transType, account, amtInPennies);
                    _context.Transaction.Add(Transaction);
                    await _context.SaveChangesAsync();
                }
                else if(account == "Loan")
                {
                    ApplicationUser.LoanBalance += amtInPennies*(-1);
                    CreateTransaction(transType, account, amtInPennies*(-1));
                    _context.Transaction.Add(Transaction);
                    await _context.SaveChangesAsync();
                }
            }
            else if(transType == "Withdraw")
            {
                if (account == "Savings" || account == "Checking")
                {
                    if (account == "Savings")
                    {
                        ApplicationUser.SavingsBalance += amtInPennies*(-1);
                    }
                    else if (account == "Checking")
                    {
                        ApplicationUser.CheckingBalance += amtInPennies*(-1);
                    }
                    CreateTransaction(transType, account, amtInPennies * (-1));
                    _context.Transaction.Add(Transaction);
                    await _context.SaveChangesAsync();
                }
                else if (account == "Loan")
                {
                    ApplicationUser.LoanBalance += amtInPennies;
                    CreateTransaction(transType, account, amtInPennies);
                    _context.Transaction.Add(Transaction);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
