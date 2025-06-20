using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class TransferResultDto
    {
        public long FromAccountNumber { get; set; }
        public long ToAccountNumber { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal Amount { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal FromAccountOldBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal FromAccountNewBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal ToAccountOldBalance { get; set; }
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal ToAccountNewBalance { get; set; }
        public string Message { get; set; }  
    }
}
