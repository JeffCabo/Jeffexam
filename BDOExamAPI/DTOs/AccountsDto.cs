using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class AccountsDto
    {
        public long CustomerId { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }

        [JsonConverter(typeof(Decimal2PlacesConverter))] 
        public decimal Balance { get; set; }
    }
}
