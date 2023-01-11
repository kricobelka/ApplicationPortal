using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class UserCompaniesViewModel
    {
        [Display(Name ="Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Company name")]
        public string CompanyName { get; set; }
    }
}