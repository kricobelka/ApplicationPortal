using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class CompanyViewModel
    {
        [Display(Name = "Reference company number")]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string BusinessId { get; set; }
    }
}
