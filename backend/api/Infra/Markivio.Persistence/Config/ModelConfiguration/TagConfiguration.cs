using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class TagDbConfiguration
{
    internal static void ConfigureTag(this ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Tag> builder = modelBuilder.Entity<Tag>();

        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder
          .Property(pre => pre.Name)
          .HasMaxLength(32);

        builder
          .Property(pre => pre.Color)
          .HasMaxLength(9);

        builder
          .HasOne(pre => pre.User)
          .WithOne()
          .HasForeignKey<Tag>("UserId")
          .IsRequired();
    }
}
