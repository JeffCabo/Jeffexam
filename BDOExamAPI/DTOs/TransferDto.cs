namespace BDOExamAPI.DTOs
{
    public class TransferDto
    {
        public long FromAccountNumber { get; set; }
        public long ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } 
        public string CreatedBy { get; set; }  
    }
}
