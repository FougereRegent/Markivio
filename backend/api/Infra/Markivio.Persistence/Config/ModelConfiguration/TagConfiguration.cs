using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class TagDbConfiguration
{
    internal static void ConfigureTag(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>();
    }
}
