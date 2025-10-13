using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class FolderDbConfiguration
{
    internal static void ConfigureFolder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folder>();
    }
}
