using ApplicationPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationPortal.Configurations
{
    public class AntennaGainConfiguration : IEntityTypeConfiguration<AntennaGain>
    {
        public void Configure(EntityTypeBuilder<AntennaGain> builder)
        {
            builder.HasKey(q => q.AntennaGainId);
            builder.Property(q => q.Amount).IsRequired();

            //обязательно указывать required для енама? тоже как бы навиг свойство
            builder.Property(q => q.AntennaGainUnit).IsRequired();

            builder.ToTable("AntennaGains");
        }
    }
}
