using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cheermate.Infrastructure.Data
{
    public class CheermateDbContextFactory : IDesignTimeDbContextFactory<CheermateDbContext>
    {
        public CheermateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CheermateDbContext>();

            // TODO: Move to configuration / user secrets for production
            const string conn =
                "Server=(localdb)\\MSSQLLocalDB;Database=CheermateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(conn);

            return new CheermateDbContext(optionsBuilder.Options);
        }
    }
}