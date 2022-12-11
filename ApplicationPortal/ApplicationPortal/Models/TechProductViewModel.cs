namespace ApplicationPortal.Models
{
    public class TechProductViewModel
    {
        public int ProductId { get; set; }

        public double FrequencyRange { get; set; }

        public string FrequencyUnit { get; set; }

        public string OutputPower { get; set; }

        public double AntennaGainAmount { get; set; }

        public string AntennaGainUnit { get; set; }
    }
}
