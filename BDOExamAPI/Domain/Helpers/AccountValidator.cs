using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Entities;
using BDOExamAPI.Domain.Enums;
using BDOExamAPI.DTOs;

namespace BDOExamAPI.Domain.Helpers
{
    public static class AccountValidator
    {
        public static ValidationResultDto ValidateFields(IAccountDto dto)
        {
            var result = new ValidationResultDto();
            var errors = new List<string>();
            var acctype = dto.AccountType;
            if (!Enum.TryParse<AccountTypeEnum>(dto.AccountType, true, out var _))
            {
                errors.Add("Invalid Account Type");
            }

            // Validate Balance is not negative
            if (dto.Balance < 0)
            {
                errors.Add("Balance cannot be negative.");
            }

            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }
        public static ValidationResultDto ValidateAccountNumber(long accountNumber)
        {
            var result = new ValidationResultDto();
            var errors = new List<string>();
            var accountNumberStr = accountNumber.ToString();

            if (accountNumberStr.Length < 8 || accountNumberStr.Length > 10)
            {
                errors.Add("Account number must be between 8 and 10 digits.");
            }
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors = errors });
        }
    }
}
