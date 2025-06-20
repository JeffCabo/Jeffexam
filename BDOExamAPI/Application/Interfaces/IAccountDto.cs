namespace BDOExamAPI.Application.Interfaces
{
    public interface IAccountDto
    {
        public long CustomerId { get; set; } 
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
