namespace ApplicationPortal.Models
{
    public class TechProductViewModel
    {
        public int ProductId { get; set; }

        public List<FrequencyViewModel> Frequencies { get; set; } = new List<FrequencyViewModel>();

        public string OutputPower { get; set; }

        public AntennaGainViewModel AntennaGain { get; set; } = new AntennaGainViewModel();
    }
}
