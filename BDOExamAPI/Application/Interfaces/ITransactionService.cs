using BDOExamAPI.DTOs;

namespace BDOExamAPI.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<DepositResultDto> DepositAsync(DepositDto dto);
        Task<WithdrawResultDto> WithdrawAsync(WithdrawDto dto);
        Task<TransferResultDto> TransferAsync(TransferDto dto);
        Task<TransactionDetailsDto?> GetTransactionByIdAsync(long id);
        Task<List<TransactionHistoryDto>> GetTransactionsByAccountNumberAsync(long accountNumber);
        Task<List<TransactionHistoryDto>> GetAccountStatementAsync(AccountStatementDto dto);
        Task<List<TransactionHistoryDto>> GetTransactionSearchResultAsync(TransactionSearchDto dto);
    }
}
