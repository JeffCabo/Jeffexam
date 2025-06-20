using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class DepositResultDto
    {
        public long AccountNumber { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal PreviousBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal DepositedAmount { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal NewBalance { get; set; }
        public string Message { get; set; }
    }
}
