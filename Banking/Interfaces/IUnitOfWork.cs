using Banking.Models;

namespace Banking.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<Transaction> Transaction { get; }

        int Commit();
        Task<int> CommitAsync();
    }
}
