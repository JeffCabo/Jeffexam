namespace BDOExamAPI.DTOs
{
    public class UpdateAccountStatusDto
    {
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; } = null!;
    }
}
