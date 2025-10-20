using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class FolderDbConfiguration
{
    internal static void ConfigureFolder(this ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Folder> builder = modelBuilder.Entity<Folder>();

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
          .WithOne()
          .HasForeignKey<Folder>("UserId")
          .OnDelete(DeleteBehavior.Cascade)
          .IsRequired(true);
    }
}
