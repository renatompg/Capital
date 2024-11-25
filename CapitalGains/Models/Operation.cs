using System.Text.Json.Serialization;

namespace CapitalGains.Models
{
    public class Operation
    {
        [JsonPropertyName("operationType")]
        public OperationType OperationType { get; set; }

        [JsonPropertyName("unitCost")]
        public decimal UnitCost { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
