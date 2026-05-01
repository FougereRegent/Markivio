using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal class ArticleDbConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder.Property(pre => pre.Title)
          .IsRequired()
          .HasMaxLength(64);

        builder
          .HasOne(pre => pre.User)
          .WithMany()
          .HasForeignKey("UserId");

        builder.ComplexProperty(pre => pre.ArticleContent);

        builder
            .HasMany(pre => pre.Tags)
            .WithMany();

        builder
            .Property(pre => pre.IsFramable)
            .HasDefaultValue(false);
    }
}
