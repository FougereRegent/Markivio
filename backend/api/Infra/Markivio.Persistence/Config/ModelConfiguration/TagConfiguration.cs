using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class TagDbConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        const string nameFkUser = "UserId";

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
          .WithMany()
          .HasForeignKey(nameFkUser)
          .IsRequired();

        builder
          .HasIndex(["Name", nameFkUser])
          .IsUnique();

        builder
          .HasQueryFilter(pre => pre.User.Id == EF.Property<Guid>(pre, "CurrentUserId"));
    }
}
