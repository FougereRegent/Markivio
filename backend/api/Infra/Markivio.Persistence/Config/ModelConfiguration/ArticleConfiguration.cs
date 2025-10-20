using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class ArticleDbConfiguration
{
    internal static void ConfigureArticle(this ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Article> builder = modelBuilder.Entity<Article>();
        builder
          .HasKey(pre => pre.Id);

        builder
          .Property(pre => pre.Id)
          .ValueGeneratedOnAdd();

        builder.Property(pre => pre.Title)
          .IsRequired()
          .HasMaxLength(64);
        builder.Property(pre => pre.Content)
          .HasMaxLength(-1);

        builder.Property(pre => pre.Source)
          .HasMaxLength(256);

        builder
          .HasOne(pre => pre.User)
          .WithMany();

        builder
          .HasMany(pre => pre.Tags)
          .WithMany();
    }
}
