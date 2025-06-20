using Azure.Core;
using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Entities;
using BDOExamAPI.Domain.Enums;
using BDOExamAPI.DTOs;
using BDOExamAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BDOExamAPI.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DepositResultDto> DepositAsync(DepositDto dto)
        {
            await using var DBTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber && a.IsActive == true);

                if (account == null)
                    throw new TransactionException("Account not found or inactive.");

                decimal oldBalance = account.Balance;

                account.Balance = account.Balance+ dto.Amount;
                account.ModifiedDate = DateTime.Now;
                account.ModifiedBy = dto.CreatedBy;

                var transactionEntity = new Domain.Entities.Transaction
                {
                    FromAccountId = account.Id,
                    ToAccountId = account.Id,
                    Amount = dto.Amount,
                    TransactionType = "Deposit",
                    Description = dto.Description,
                    Timestamp = DateTime.Now,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    ModifiedBy = dto.CreatedBy,
                    ModifiedDate = DateTime.Now
                };

                var log = new TransactionLog
                {
                    AccountId = account.Id,
                    Transaction = transactionEntity,
                    Operation = "Deposit",
                    Amount = dto.Amount,
                    OldBalance = oldBalance,
                    NewBalance = account.Balance,
                    Description = dto.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy
                };

                _context.Transactions.Add(transactionEntity);
                _context.TransactionLog.Add(log);

                await _context.SaveChangesAsync();
                await DBTransaction.CommitAsync();

                return new DepositResultDto
                {
                    AccountNumber = account.AccountNumber,
                    PreviousBalance = oldBalance,
                    DepositedAmount = dto.Amount,
                    NewBalance = account.Balance,
                    Message = "Deposit successful"
                };
            }
            catch (Exception)
            {
                await DBTransaction.RollbackAsync();
                throw;
            }
        }
        public async Task<WithdrawResultDto> WithdrawAsync(WithdrawDto dto)
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber && a.IsActive);

                if (account == null)
                    throw new TransactionException("Account not found or inactive."); 

                if (account.Balance < dto.Amount)
                    throw new TransactionException("Insufficient balance.");

                decimal oldBalance = account.Balance;
                account.Balance = account.Balance - dto.Amount;
                account.ModifiedDate = DateTime.Now;
                account.ModifiedBy = dto.CreatedBy;

                var transactionEntity = new Domain.Entities.Transaction
                {
                    FromAccountId = account.Id,
                    ToAccountId = account.Id,
                    Amount = dto.Amount,
                    TransactionType = "Withdrawal",
                    Description = dto.Description,
                    Timestamp = DateTime.Now,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    ModifiedBy = dto.CreatedBy,
                    ModifiedDate = DateTime.Now
                };

                var log = new TransactionLog
                {
                    AccountId = account.Id,
                    Transaction = transactionEntity,
                    Operation = "Withdrawal",
                    Amount = dto.Amount,
                    OldBalance = oldBalance,
                    NewBalance = account.Balance,
                    Description = dto.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy
                };

                _context.Transactions.Add(transactionEntity);
                _context.TransactionLog.Add(log);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return new WithdrawResultDto
                {
                    AccountNumber = account.AccountNumber,
                    OldBalance = oldBalance,
                    NewBalance = account.Balance,
                    Amount = dto.Amount,
                    Message = "Withdrawal successful."
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
        public async Task<TransferResultDto> TransferAsync(TransferDto dto)
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var fromAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == dto.FromAccountNumber && a.IsActive);

                var toAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == dto.ToAccountNumber && a.IsActive);

                if (fromAccount == null )
                    throw new TransactionException("Sender account not found or inactive.");

                if ( toAccount == null)
                    throw new TransactionException("Destination accounts not found or inactive.");

                if (fromAccount.Id == toAccount.Id)
                    throw new TransactionException("Cannot transfer to the same account.");
                 
                if (fromAccount.Balance < dto.Amount)
                    throw new TransactionException("Insufficient balance in source account.");

                decimal fromOldBalance = fromAccount.Balance;
                decimal toOldBalance = toAccount.Balance;

                fromAccount.Balance -= dto.Amount;
                fromAccount.ModifiedDate = DateTime.Now;
                fromAccount.ModifiedBy = dto.CreatedBy;

                toAccount.Balance += dto.Amount;
                toAccount.ModifiedDate = DateTime.Now;
                toAccount.ModifiedBy = dto.CreatedBy;

                var transaction = new Domain.Entities.Transaction
                {
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    Amount = dto.Amount,
                    TransactionType = "Transfer",
                    Description = dto.Description,
                    Timestamp = DateTime.Now,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    ModifiedBy = dto.CreatedBy,
                    ModifiedDate = DateTime.Now
                };

                var fromLog = new TransactionLog
                {
                    AccountId = fromAccount.Id,
                    Transaction = transaction,
                    Operation = "Transfer (Debit)",
                    Amount = dto.Amount,
                    OldBalance = fromOldBalance,
                    NewBalance = fromAccount.Balance,
                    Description = dto.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy
                };

                var toLog = new TransactionLog
                {
                    AccountId = toAccount.Id,
                    Transaction = transaction,
                    Operation = "Transfer (Credit)",
                    Amount = dto.Amount,
                    OldBalance = toOldBalance,
                    NewBalance = toAccount.Balance,
                    Description = dto.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dto.CreatedBy
                };

                _context.Transactions.Add(transaction);
                _context.TransactionLog.AddRange(fromLog, toLog);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return new TransferResultDto
                {
                    FromAccountNumber = dto.FromAccountNumber,
                    ToAccountNumber = dto.ToAccountNumber,
                    Amount = dto.Amount,
                    FromAccountOldBalance = fromOldBalance,
                    FromAccountNewBalance = fromAccount.Balance,
                    ToAccountOldBalance = toOldBalance,
                    ToAccountNewBalance = toAccount.Balance,
                    Message="Transafer successful"
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
        public async Task<TransactionDetailsDto?> GetTransactionByIdAsync(long id)
        {
            var transaction = await _context.Transactions
                             .Include(t => t.FromAccount)
                             .Include(t => t.ToAccount)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) 
                throw new TransactionException("Transaction not found.");

            return new TransactionDetailsDto
            {
                Id = transaction.Id,
                FromAccountNumber = transaction.FromAccount.AccountNumber,
                ToAccountNumber = transaction.ToAccount?.AccountNumber,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Description = transaction.Description,
                Timestamp = transaction.Timestamp,
                Status = ((TransactionStatusEnum)transaction.Status).ToString(),
                CreatedBy = transaction.CreatedBy
            };
        }
        public async Task<List<TransactionHistoryDto>> GetTransactionsByAccountNumberAsync(long accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                throw new TransactionException("Account not found or inactive.");


            var transactions = await _context.Transactions
                     .Include(t => t.FromAccount)
                     .Include(t => t.ToAccount)
                     .Where(t =>
                         t.FromAccount.AccountNumber == accountNumber ||
                         (t.ToAccount != null && t.ToAccount.AccountNumber == accountNumber))
                     .OrderByDescending(t => t.Timestamp) 
                     .Select(t => new TransactionHistoryDto
                     {
                         Id = t.Id,
                         TransactionType = t.TransactionType,
                         Amount = t.Amount,
                         Timestamp = t.Timestamp,
                         Description = t.Description,
                         FromAccountNumber = t.FromAccount.AccountNumber,
                         ToAccountNumber = t.ToAccount != null ? t.ToAccount.AccountNumber : null,
                         Status = ((TransactionStatusEnum)t.Status).ToString()
                     })
                     .ToListAsync();


            return transactions;
        }
        public async Task<List<TransactionHistoryDto>> GetAccountStatementAsync(AccountStatementDto dto)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber);

            if (account == null)
                throw new TransactionException("Account not found or inactive.");

            var startDate = dto.StartDate.Date;
            var endDate = dto.EndDate.Date.AddDays(1).AddTicks(-1);  

            var transactions = await _context.Transactions
                     .Include(t => t.FromAccount)
                     .Include(t => t.ToAccount)
                     .Where(t =>
                         (t.FromAccount.AccountNumber == dto.AccountNumber ||
                         (t.ToAccount != null && t.ToAccount.AccountNumber == dto.AccountNumber))
                         && t.Timestamp >= startDate && t.Timestamp <= endDate)
                     .OrderByDescending(t => t.Timestamp)
                     .Select(t => new TransactionHistoryDto
                     {
                         Id = t.Id,
                         TransactionType = t.TransactionType,
                         Amount = t.Amount,
                         Timestamp = t.Timestamp,
                         Description = t.Description,
                         FromAccountNumber = t.FromAccount.AccountNumber,
                         ToAccountNumber = t.ToAccount != null ? t.ToAccount.AccountNumber : null,
                         Status = ((TransactionStatusEnum)t.Status).ToString()
                     })
                     .ToListAsync();


            return transactions;
        }
        public async Task<List<TransactionHistoryDto>> GetTransactionSearchResultAsync(TransactionSearchDto dto)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber);

            if (account == null)
                throw new TransactionException("Account not found or inactive.");

            

            var transactions = await _context.Transactions
                     .Include(t => t.FromAccount)
                     .Include(t => t.ToAccount)
                     .Where(t =>
                             (t.FromAccount.AccountNumber == dto.AccountNumber ||
                             (t.ToAccount != null && t.ToAccount.AccountNumber == dto.AccountNumber))  &&
                             ((dto.Amount == null || t.Amount == dto.Amount) ||
                             (dto.Description == null || (t.Description.ToLower().Contains(dto.Description.ToLower()))))
                         )
                     .OrderByDescending(t => t.Timestamp)
                     .Select(t => new TransactionHistoryDto
                     {
                         Id = t.Id,
                         TransactionType = t.TransactionType,
                         Amount = t.Amount,
                         Timestamp = t.Timestamp,
                         Description = t.Description,
                         FromAccountNumber = t.FromAccount.AccountNumber,
                         ToAccountNumber = t.ToAccount != null ? t.ToAccount.AccountNumber : null,
                         Status = ((TransactionStatusEnum)t.Status).ToString()
                     })
                     .ToListAsync();


            return transactions;
        }

    }
}
