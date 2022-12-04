using ApplicationPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationPortal.Configurations
{
    public class FrequencyConfiguration : IEntityTypeConfiguration<Frequency>
    {
        public void Configure(EntityTypeBuilder<Frequency> builder)
        {
            builder.HasKey(q => q.FrequencyId);
            builder.Property(q => q.Range).IsRequired();
            builder.Property(q => q.FrequencyUnit).IsRequired();

            builder.ToTable("Frequencies");
            //макс размер енама не смогу указать верно?
        }
    }
}

