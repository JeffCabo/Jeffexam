using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Enums;

namespace BDOExamAPI.DTOs
{
    public class CreateAccountDto : IAccountDto
    {
        public long CustomerId { get; set; } 
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string ProccessBy { get; set; } 

    }
}
