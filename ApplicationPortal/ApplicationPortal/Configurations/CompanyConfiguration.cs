using ApplicationPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationPortal.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(q => q.CompanyId);
            builder.Property(q => q.CompanyName).HasMaxLength(30).IsRequired();
            builder.Property(q => q.Address).HasMaxLength(30).IsRequired();
            //только в случае добавлеия компании обязательно нужно будет вводить данные лицензии
            //при выборе компании будет высвечивваться только нпзвание и адрес компании
            builder.Property(q => q.BusinessId).HasMaxLength(30).IsRequired();

            builder.HasMany(q => q.Users)
                .WithOne(q => q.Company)
                .HasForeignKey(q => q.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Companies");
        }
    }
}
