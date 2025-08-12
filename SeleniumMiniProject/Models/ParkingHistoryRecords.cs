
namespace SeleniumTests.Models
{
    public class ParkingHistoryRecords
    {
        public string CarPlate { get; set; }
        public string Slot { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Fee { get; set; }
        public string? Image { get; set; }
    }
}
