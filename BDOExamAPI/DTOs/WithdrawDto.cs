namespace BDOExamAPI.DTOs
{
    public class WithdrawDto
    {
        public long AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; }  
    }
}
