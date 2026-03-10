using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class FolderDbConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder
          .Property(pre => pre.Name)
          .HasMaxLength(32);

        builder
          .HasOne(pre => pre.User)
          .WithMany()
          .HasForeignKey("UserId")
          .OnDelete(DeleteBehavior.Cascade)
          .IsRequired(true);
    }
}
