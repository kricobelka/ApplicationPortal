using System.ComponentModel.DataAnnotations;
namespace ApplicationPortal.Models
{
    public class TechProductViewModel
    {
        public int ProductId { get; set; }

        //писать атрибуты в модели freuqeucnyviewmodel?
        public List<FrequencyViewModel> Frequencies { get; set; } = new List<FrequencyViewModel>();

        [Display(Name = "Output power")]
        [Required(ErrorMessage = "Please insert actual output power (dbm)")]
        public string OutputPower { get; set; }

        //нужно ли писать required and display 
        
        public AntennaGainViewModel AntennaGain { get; set; } = new AntennaGainViewModel();
    }
}
