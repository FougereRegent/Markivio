using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.Schema;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class ArticleDbConfiguration
{
    internal static void ConfigureArticle(this ModelBuilder modelBuilder, IAuthUser authUser)
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

        builder
          .HasOne(pre => pre.User)
          .WithMany()
          .HasForeignKey("UserId");

        builder
          .OwnsOne(pre => pre.ArticleContent, sa =>
          {
              sa.ToJson();
              sa.OwnsMany(pre => pre.Tags);
          });

        builder.HasQueryFilter(pre => pre.User.Id == Guid.NewGuid());
        builder
          .HasQueryFilter(pre => pre.User.AuthId == authUser.CurrentUser.AuthId);
    }
}
