using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class UserDbConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder.ComplexProperty(pre => pre.Email);

        builder.ComplexProperty(pre => pre.Identity, buildAction =>
        {
            buildAction.Property(pre => pre.FirstName)
            .HasMaxLength(128);
            buildAction
              .Property(pre => pre.Username)
              .HasMaxLength(128);
            buildAction
              .Property(pre => pre.LastName)
              .HasMaxLength(128);
        });

        builder
          .Property(pre => pre.AuthId);

        builder
          .HasIndex(pre => pre.AuthId);
    }
}
