using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.DTOs;
using System.Text.RegularExpressions;

namespace BDOExamAPI.Domain.Helpers
{
    public static class CustomerValidator
    {
        public static ValidationResultDto ValidateFields(ICustomerDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                errors.Add("FirstName cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                errors.Add("LastName cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                errors.Add("Email cannot be empty.");
            }
            else if (!IsValidEmail(dto.Email))
            {
                errors.Add("Email format is invalid.");
            }

            if (string.IsNullOrWhiteSpace(dto.Phone))
            {
                errors.Add("Phone cannot be empty.");
            }
            else if (!IsNumeric(dto.Phone))
            {
                errors.Add("Phone must contain only numbers.");
            }
             
            return (new ValidationResultDto { IsValid = errors.Count == 0, Errors= errors } );
        }

        private static bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }
    }
}
