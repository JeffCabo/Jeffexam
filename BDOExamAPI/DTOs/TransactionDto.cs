using BDOExamAPI.Domain.Enums;
using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public long FromAccountId { get; set; }
        public long? ToAccountId { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionStatusEnum Status { get; set; }
    }
}
