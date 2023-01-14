using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class ExtraProductViewModel
    {
        public int ProductId { get; set; }
        
        [Display(Name = "Additional information about the product (if any)")]
        public string? OtherInformation { get; set; }

        [Required(ErrorMessage = "Please provide link to the documents")]
        [Display(Name = "Path to file")]
        [Url]
        public string PathToFile { get; set; }
    }
}
