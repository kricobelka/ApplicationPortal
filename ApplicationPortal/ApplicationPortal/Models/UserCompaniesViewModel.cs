using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class UserCompaniesViewModel
    {
        public string UserEmail { get; set; }
        public string CompanyName { get; set; }
    }
}