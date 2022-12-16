using ApplicationPortal.Enums;

namespace ApplicationPortal.Data.Entities
{
    public class Frequency
    {
        public int FrequencyId { get; set; }
        public double StartRange { get; set; }
        public double EndRange { get; set; }

        public FrequencyUnit FrequencyUnit { get; set; }
        
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
