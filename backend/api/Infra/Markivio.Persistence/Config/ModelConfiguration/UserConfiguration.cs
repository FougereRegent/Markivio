using Markivio.Domain.Entities;
using Markivio.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class UserDbConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
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
          .Property(pre => pre.AuthId);

        builder
          .Property(pre => pre.LastName)
          .HasMaxLength(128);

        builder
          .HasIndex(pre => pre.AuthId);
    }
}
