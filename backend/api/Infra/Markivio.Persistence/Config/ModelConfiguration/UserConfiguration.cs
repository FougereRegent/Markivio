using Markivio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Markivio.Persistence.Config.ModelConfiguration;

internal static class UserDbConfiguration
{
    internal static void ConfigureUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
    }
}
