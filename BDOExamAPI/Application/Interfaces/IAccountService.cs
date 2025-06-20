using BDOExamAPI.DTOs;

namespace BDOExamAPI.Application.Interfaces
{
    public interface IAccountService
    {
        Task<long> CreateAccountAsync(CreateAccountDto dto);
        Task<AccountDetailsDto?> GetAccountDetailsAsync(long accountNumber);
        Task<BalanceDto> GetAccountBalanceAsync(long accountNumber);
        Task<bool> UpdateAccountStatusAsync(long accountNumber, UpdateAccountStatusDto dto);
    }
}
