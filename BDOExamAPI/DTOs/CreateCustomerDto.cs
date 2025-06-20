using BDOExamAPI.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BDOExamAPI.DTOs
{
    public class CreateCustomerDto : ICustomerDto
    { 
        public string FirstName { get; set; }  
        public string LastName { get; set; }  
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string ProccessBy { get; set; }
    }
}
