namespace BDOExamAPI.DTOs
{
    public class CustomersDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<AccountsDto> Accounts { get; set; }
    }
}
