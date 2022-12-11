namespace ApplicationPortal.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        //будет выбор между Mhz/Khz

        public ICollection<Frequency> Frequencies { get; set; }

        public string OutputPower { get; set; }    

        public string OtherInformation { get; set; }

        public string PathToFile { get; set; }

        public AntennaGain AntennaGain { get; set; }
    }
}
