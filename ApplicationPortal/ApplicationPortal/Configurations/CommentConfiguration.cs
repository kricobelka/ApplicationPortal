using ApplicationPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationPortal.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(q => q.CommentId);

            builder.Property(q => q.Text).HasMaxLength(100).IsRequired();
            builder.Property(q => q.CreatedDate).IsRequired();
            builder.Property(q => q.UserId).IsRequired();
            builder.HasOne(q => q.User)
                .WithMany(q => q.Comments)
                .HasForeignKey(q => q.UserId);

            builder.HasOne(q => q.Product)
                .WithMany(q => q.Comments)
                .HasForeignKey(q => q.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Comments");
        }
    }
}
