namespace PositionsService.Models
{
    public class RateChangeEvent
    {
        public string Symbol { get; set; }  // Example: "EUR/USD"
        public decimal NewRate { get; set; }
    }
}
