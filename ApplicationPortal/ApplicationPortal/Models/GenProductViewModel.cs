using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class GenProductViewModel
    {
        public int? ProductId { get; set; }

        //public string ?UserName { get; set; }
        [Required (ErrorMessage = "Please type name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please type model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please type brand")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Please type manufacturer")]
        public string Manufacturer { get; set; }
    }
}
