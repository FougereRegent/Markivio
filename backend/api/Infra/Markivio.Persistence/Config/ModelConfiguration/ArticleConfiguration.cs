using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class ArticleDbConfiguration
{
    internal static void ConfigureArticle(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>();
    }
}
