using ApplicationPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Ref. no.:")]
        public int Id { get; set; }

        //adding username
        [Display(Name = "User name:")]
        public string UserName { get; set; }

        [Display(Name = "Name:")]
        public string Name { get; set; }

        [Display(Name = "Model:")]
        public string Model { get; set; }

        [Display(Name = "Brand:")]
        public string Brand { get; set; }

        [Display(Name = "Manufacturer:")]
        public string Manufacturer { get; set; }

        public List<FrequencyViewModel> Frequencies { get; set; }

        [Display(Name = "Output power:")]
        public string OutputPower { get; set; }

        public AntennaGainViewModel AntennaGain { get; set; }

        [Display(Name = "Additional information:")]
        public string OtherInformation { get; set; }

        [Display(Name = "Path to file:")]
        public string PathToFile { get; set; }

        [Display(Name = "Status:")]
        public ProductStatus? Status { get; set; }
    }
}