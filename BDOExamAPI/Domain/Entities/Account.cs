namespace BDOExamAPI.Domain.Entities
{
    public class Account
    {
        public long Id { get; set; }
        public long AccountNumber { get; set; }
        public long CustomerId { get; set; }
        public string AccountType { get; set; } = null!;
        public decimal Balance { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public Customer Customer { get; set; } = null!;
        public ICollection<Transaction> FromTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> ToTransactions { get; set; } = new List<Transaction>();

    }
}
