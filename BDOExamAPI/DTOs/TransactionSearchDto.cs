namespace BDOExamAPI.DTOs
{
    public class TransactionSearchDto
    {
        public long AccountNumber { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
    }
}
