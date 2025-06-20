namespace BDOExamAPI.Domain.Entities
{
    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
