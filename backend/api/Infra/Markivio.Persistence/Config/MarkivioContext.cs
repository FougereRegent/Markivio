#nullable disable
using Microsoft.EntityFrameworkCore;
using Markivio.Domain.Entities;
using Markivio.Persistence.Config.ModelConfiguration;
using Markivio.Domain.Auth;

namespace Markivio.Persistence.Config;

public class MarkivioContext : DbContext
{
    private readonly IAuthUser authUser;
    public DbSet<User> User { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Folder> Folder { get; set; }

    public MarkivioContext(DbContextOptions options, IAuthUser authUser) : base(options)
    {
        this.authUser = authUser;
    }

    public MarkivioContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureUser();
        modelBuilder.ConfigureArticle(authUser);
        modelBuilder.ConfigureFolder();
        modelBuilder.ConfigureTag();
    }
}
