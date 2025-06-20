namespace BDOExamAPI.Domain.Enums
{
    public enum AccountTypeEnum
    {
        Checking,
        Savings
    }

    public enum TransactionType
    {
      Deposit,
      Withdrawal,
      Transfer
    }

    public enum AccountStatus
    {
        Active = 1,
        NotActive=0
    }
}
