using System.Text.Json.Serialization;
namespace CapitalGains.Models
{
    public class TaxResult
    {
        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }
    }
}