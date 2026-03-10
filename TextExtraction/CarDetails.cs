using System.Text.Json.Serialization;

namespace TextExtraction
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AvailabilityType
    {
        Sale,
        Lease,
        Rent
    }
    internal class CarDetails
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int? Year { get; set; }
        public double? Mileage { get; set; }
        public double? Price { get; set; }
        public AvailabilityType? AvailabilityType { get; set; }
        public double? PricePerMonth { get; set; }
        public double? PricePerDay { get; set; }
        public string[]? Features { get; set; }
        public string? Location { get; set; }
        public string ShortSummary { get; set; } = string.Empty;
        public int? OwnerCount { get; set; }
    }
}
