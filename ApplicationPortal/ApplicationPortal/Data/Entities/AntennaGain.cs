using ApplicationPortal.Enums;

namespace ApplicationPortal.Data.Entities
{
    public class AntennaGain
    {
        public int AntennaGainId { get; set; }
        public double Amount { get; set; }
        public AntennaGainUnit AntennaGainUnit { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
