using BDOExamAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDOExamAPI.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Account> Accounts { get; }
        DbSet<Transaction> Transactions { get; }
        DbSet<TransactionLog> TransactionLog { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
