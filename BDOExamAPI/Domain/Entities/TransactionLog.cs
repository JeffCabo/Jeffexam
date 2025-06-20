namespace BDOExamAPI.Domain.Entities
{
    public class TransactionLog
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public long? TransactionId { get; set; }
        public Transaction? Transaction { get; set; }
        public string Operation { get; set; } = null!;  
        public decimal Amount { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
