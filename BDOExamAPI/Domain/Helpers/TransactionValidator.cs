using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.DTOs;

namespace BDOExamAPI.Domain.Helpers
{
    public class TransactionValidator
    {
        public static ValidationResultDto ValidateDeposit(DepositDto dto)
        {
            var errors = new List<string>();
            var accountNumberStr = dto.AccountNumber.ToString();

            if (dto.AccountNumber == 0)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }
            else if (accountNumberStr.Length < 8 || accountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            if (dto.Amount <= 0)
            {
                errors.Add("Invalid amount");
            } 
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }
        public static ValidationResultDto ValidateTransfer(TransferDto dto)
        {
            var errors = new List<string>();
            var toAccountNumberStr = dto.ToAccountNumber.ToString();

            if (dto.ToAccountNumber == 0)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }
            else if (toAccountNumberStr.Length < 8 || toAccountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            var fromAccountNumberStr = dto.FromAccountNumber.ToString();

            if (dto.ToAccountNumber == 0)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }
            else if (fromAccountNumberStr.Length < 8 || fromAccountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            if (dto.Amount <= 0)
            {
                errors.Add("Invalid amount");
            }
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }

        public static ValidationResultDto ValidateWidrawal(WithdrawDto dto)
        {
            var errors = new List<string>();
            var accountNumberStr = dto.AccountNumber.ToString();

            if (dto.AccountNumber == 0)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }
            else if (accountNumberStr.Length < 8 || accountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            if (dto.Amount <= 0)
            {
                errors.Add("Invalid amount");
            }
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }

        public static ValidationResultDto ValidateId(long id)
        {
            var errors = new List<string>();  
            if (id <= 0)
            {
                errors.Add("Invalid ID.");
            }
            
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }

        public static ValidationResultDto ValidateAccountStatement(AccountStatementDto dto)
        {
            var errors = new List<string>();
            var accountNumberStr = dto.AccountNumber.ToString();

            if (accountNumberStr.Length < 8 || accountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            if (dto.StartDate == null)
            {
                errors.Add("Start date is required.");
            }

            if (dto.EndDate == null)
            {
                errors.Add("End date is required.");
            }

            if (dto.StartDate != null && dto.EndDate != null && dto.StartDate > dto.EndDate)
            {
                errors.Add("Start date must be before or equal to end date.");
            }


            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }

        public static ValidationResultDto ValidateSearchTransaction(TransactionSearchDto dto)
        {
            var errors = new List<string>();
            var accountNumberStr = dto.AccountNumber.ToString();

            if (accountNumberStr.Length < 8 || accountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }

            if (dto.Amount == null && string.IsNullOrWhiteSpace(dto.Description))
            {
                errors.Add("At least one of amount or description must be provided.");
            }


            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }
    }
}
