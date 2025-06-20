using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class AccountDetailsDto
    {
        public long Id { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }

        [JsonConverter(typeof(Decimal2PlacesConverter))] 
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TransactionDto> RecentTransactions { get; set; }
    }
}
