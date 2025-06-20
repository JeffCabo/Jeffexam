namespace BDOExamAPI.Application.Interfaces
{
    public interface ICustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProccessBy { get; set; }
    }
}
