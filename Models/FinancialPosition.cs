using PositionsService.Enum;

namespace PositionsService.Models
{
    public class FinancialPosition
    {
        public string InstrumentId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal InitialRate { get; set; }
        public PositionSideEnum Side { get; set; }
        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    }
}
