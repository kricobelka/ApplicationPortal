using ApplicationPortal.Enums;

namespace ApplicationPortal.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }

        public double FrequencyRange { get; set; }

        public string FrequencyUnit { get; set; }

        public string OutputPower { get; set; }

        public double AntennaGainAmount { get; set; }

        public string AntennaGainUnit { get; set; }

        public string OtherInformation { get; set; }

        public string PathToFile { get; set; }

    }
}