namespace BDOExamAPI.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public long FromAccountId { get; set; }
        public long? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public Account FromAccount { get; set; } = null!;
        public Account? ToAccount { get; set; } 
    }
}
