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

        public List<FrequencyViewModel> Frequencies { get; set; }

        public string OutputPower { get; set; }

        public AntennaGainViewModel AntennaGain { get; set; }

        public string OtherInformation { get; set; }

        public string PathToFile { get; set; }

        public string Status { get; set; }

    }
}