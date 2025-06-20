using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class WithdrawResultDto
    {
        public long AccountNumber { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal OldBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal NewBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal Amount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
