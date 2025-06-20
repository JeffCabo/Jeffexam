using BDOExamAPI.Application.Exceptions;
using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Entities;
using BDOExamAPI.Domain.Enums;
using BDOExamAPI.DTOs;
using BDOExamAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BDOExamAPI.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAccountAsync(CreateAccountDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                throw new AccountException("Customer not found");

            var newAccountNumber = await CreateAccountNumberAsync();

            if (newAccountNumber == 0)
                throw new AccountException("Unable to create Account Number");
            var account = new Account
            {
                CustomerId = dto.CustomerId,
                AccountNumber = newAccountNumber,
                AccountType = dto.AccountType.ToString(),
                Balance = dto.Balance,
                IsActive = true,
                CreatedBy = dto.ProccessBy,
                ModifiedBy = dto.ProccessBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return newAccountNumber;
        }

        public async Task<AccountDetailsDto?> GetAccountDetailsAsync(long accountNumber)
        {
            // include the recent transaction within the month and account is active

            var fifteenDaysAgo = DateTime.Now.AddDays(-15);

            var account = await _context.Accounts
                .Include(a => a.FromTransactions)
                .Include(a => a.ToTransactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return null;

            var transactions = account.FromTransactions
                .Concat(account.ToTransactions)
                .Where(t => t.Timestamp >= fifteenDaysAgo)
                .OrderByDescending(t => t.Timestamp)
                .DistinctBy(t => t.Id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    Timestamp = t.Timestamp,
                    FromAccountId = t.FromAccountId,
                    ToAccountId = t.ToAccountId,
                    Description = t.Description,
                    Status = (TransactionStatusEnum)t.Status
                })
                .ToList();

            return new AccountDetailsDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                Balance = account.Balance,
                RecentTransactions = transactions,
                IsActive = account.IsActive,
                CreatedDate = account.CreatedDate
            };
        }

        public async Task<BalanceDto> GetAccountBalanceAsync(long accountNumber)
        {
            var result = new BalanceDto();
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                result.Balance = account.Balance;
            }
            else
            {
                result.Balance = null;
            }


            return result;
        }

        public async Task<bool> UpdateAccountStatusAsync(long accountNumber, UpdateAccountStatusDto dto)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null) return false;
            //assuming getting the username from jwt token claims
            account.ModifiedBy = "admintest";
            account.ModifiedDate = DateTime.UtcNow;
            account.IsActive = dto.IsActive;
            

            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<long> CreateAccountNumberAsync()
        {
            string today = DateTime.Now.ToString("yyMMdd");

            var newSequence = new AccountNumberSequence
            {
                SequenceDate = DateTime.Now
            };

            _context.AccountNumberSequences.Add(newSequence);
            await _context.SaveChangesAsync();


            string idPart;

            if (newSequence.Id < 10)
            {
                idPart = "0" + newSequence.Id;
            }
            else if (newSequence.Id <= 9999)
            {
                idPart = newSequence.Id.ToString();
            }
            else
            {
                idPart = (newSequence.Id % 10000).ToString("D4");
            }

            string accountNumberStr = today + idPart;

            return long.Parse(accountNumberStr);

        }
    }
}
