using BDOExamAPI.Domain.Helpers;
using System.Text.Json.Serialization;

namespace BDOExamAPI.DTOs
{
    public class BalanceDto
    {
        [JsonConverter(typeof(Decimal2PlacesConverter))]
        public decimal? Balance { get; set; }
    }
}
