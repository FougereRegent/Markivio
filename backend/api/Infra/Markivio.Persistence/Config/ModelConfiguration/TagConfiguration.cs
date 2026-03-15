using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class TagDbConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        const string nameFkUser = "UserId";

		builder.ToTable("tags");
        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();


		builder.ComplexProperty(pre => pre.TagValue);

        builder
          .HasOne(pre => pre.User)
          .WithMany()
          .HasForeignKey(nameFkUser)
          .IsRequired();

        builder
          .HasQueryFilter(pre => pre.User.Id == EF.Property<Guid>(pre, "CurrentUserId"));
    }
}
