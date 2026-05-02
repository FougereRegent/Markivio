using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Markivio.Persistence.Config
{
    public class MarkivioContextFactory : IDesignTimeDbContextFactory<MarkivioContext>
    {
        public MarkivioContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MarkivioContext>();

            // Connection string hardcodée pour le design time
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MarkivioDb;Username=postgres;Password=YourPassword123")
                          .UseCamelCaseNamingConvention();

            // À design time, on n'a pas d'utilisateur courant, on peut passer null
            return new MarkivioContext(optionsBuilder.Options, authUser: null);
        }
    }
}
