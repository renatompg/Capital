using System.Text.Json.Serialization;

namespace CapitalGains.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationType
    {
        Buy,
        Sell
    }
}