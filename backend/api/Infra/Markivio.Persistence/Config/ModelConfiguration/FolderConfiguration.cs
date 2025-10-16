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
          .Property(pre => pre.Name)
          .HasMaxLength(32);

        builder
          .HasOne(pre => pre.User)
          .WithOne()
          .HasForeignKey<User>(pre => pre.Id)
          .OnDelete(DeleteBehavior.Cascade)
          .IsRequired();

        builder
          .HasMany(pre => pre.Articles)
          .WithOne(pre => pre.Folder)
          .OnDelete(DeleteBehavior.SetNull)
          .IsRequired(false);
    }
}
