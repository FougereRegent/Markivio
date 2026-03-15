#nullable disable
using Microsoft.EntityFrameworkCore;
using Markivio.Domain.Entities;
using Markivio.Persistence.Config.ModelConfiguration;
using Markivio.Domain.Auth;

namespace Markivio.Persistence.Config;

public class MarkivioContext : DbContext
{
    public readonly IAuthUser authUser;
	public string CurrentUserId => authUser?.CurrentUser?.AuthId ?? "";

    public DbSet<User> User { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Folder> Folder { get; set; }

    public MarkivioContext(DbContextOptions<MarkivioContext> options, IAuthUser authUser) : base(options)
    {
		this.authUser = authUser;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		optionsBuilder.AddInterceptors(new DateTimeSaveUpdateIntercpetor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		new ArticleDbConfiguration()
			.Configure(modelBuilder.Entity<Article>());
		new FolderDbConfiguration()
			.Configure(modelBuilder.Entity<Folder>());
		new UserDbConfiguration()
			.Configure(modelBuilder.Entity<User>());
		new TagDbConfiguration()
			.Configure(modelBuilder.Entity<Tag>());

		modelBuilder.Entity<Tag>()
			.HasQueryFilter(pre => pre.User.AuthId == CurrentUserId);
		modelBuilder.Entity<Article>()
			.HasQueryFilter(pre => pre.User.AuthId == CurrentUserId);
    }
}
