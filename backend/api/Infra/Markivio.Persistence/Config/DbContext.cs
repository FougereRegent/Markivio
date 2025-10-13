#nullable disable
using Microsoft.EntityFrameworkCore;
using Markivio.Domain.Entities;
using Markivio.Persistence.Config.ModelConfiguration;

namespace Markivio.Persistence.Config;

public class MarkivioContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Folder> Folder { get; set; }

    public MarkivioContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureUser();
        modelBuilder.ConfigureArticle();
        modelBuilder.ConfigureFolder();
        modelBuilder.ConfigureTag();
    }
}
