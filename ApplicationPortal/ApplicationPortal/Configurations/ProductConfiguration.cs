using ApplicationPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationPortal.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Name).HasMaxLength(30).IsRequired();
            builder.Property(q => q.Model).HasMaxLength(20).IsRequired();
            builder.Property(q => q.Manufacturer).HasMaxLength(50).IsRequired();
            builder.Property(q => q.Brand).HasMaxLength(20).IsRequired();

            //возм-ть добавления нескольких частот
            //в случае с антенной и частотами, прописывать ли тут конфигурацию?
            //или отдельные конфигурации для каждого класса антенны и частоты
            //отдельные конфигурации прописывать
            
            //builder.Property(q => q.Frequencies).IsRequired();
            builder.Property(q => q.OutputPower).HasMaxLength(10);
            //builder.Property(q => q.AntennaGain).IsRequired();
            builder.Property(q => q.PathToFile).HasMaxLength(100);
            builder.Property(q => q.OtherInformation).HasMaxLength(100);
            builder.HasIndex(q => new { q.Name, q.Model }).IsUnique();
            
            builder.HasMany(q => q.Frequencies)
                .WithOne(q => q.Product)
                .HasForeignKey(q => q.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.AntennaGain)
                .WithOne(q => q.Product)
                .HasForeignKey<AntennaGain>(q => q.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Products");
        }
    }
}
