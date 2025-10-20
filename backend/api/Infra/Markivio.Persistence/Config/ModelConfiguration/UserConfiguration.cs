using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class UserDbConfiguration
{
    internal static void ConfigureUser(this ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<User> builder = modelBuilder.Entity<User>();

        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder
          .Property(pre => pre.Email)
          .HasMaxLength(128);

        builder
          .Property(pre => pre.Username)
          .HasMaxLength(128);

        builder
          .Property(pre => pre.FirstName)
          .HasMaxLength(128);

        builder
          .Property(pre => pre.LastName)
          .HasMaxLength(128);
    }
}
