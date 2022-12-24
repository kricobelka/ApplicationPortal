using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class ExtraProductViewModel
    {
        public int ProductId { get; set; }
        
        [Display(Name = "Additional information about the product")]
        public string? OtherInformation { get; set; }

        [Required(ErrorMessage = "Please provide the link to the documents")]
        [Display(Name = "Path to file")]
        public string PathToFile { get; set; }
    }
}
