﻿using ApplicationPortal.Enums;

namespace ApplicationPortal.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }

        public ICollection<Frequency> Frequencies { get; set; }

        public string OutputPower { get; set; }    

        public string OtherInformation { get; set; }

        public string PathToFile { get; set; }

        public AntennaGain AntennaGain { get; set; }

        public ProductStatus? Status { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
