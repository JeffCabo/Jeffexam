using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class TransactionHistoryDto
    {
        public long Id { get; set; }
        public string TransactionType { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Description { get; set; }
        public long? FromAccountNumber { get; set; }
        public long? ToAccountNumber { get; set; }
        public string Status { get; set; }  
    }
}
