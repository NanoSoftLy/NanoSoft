using Microsoft.EntityFrameworkCore;
using NanoSoft.EntityFramework.Identity;

namespace NanoSoft.IdentityServer
{
    public class IdentityDbContext : NanoSoftIdentityDbContext<IdentityUser>
    {
        public const string MigrationHistoryTable = "__IdentityEFMigrationsHistory";

        public IdentityDbContext()
        {

        }

        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer("",
                options => options.MigrationsHistoryTable(MigrationHistoryTable));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
