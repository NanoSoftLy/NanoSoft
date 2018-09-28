using Microsoft.EntityFrameworkCore;
using NanoSoft.Identity;
using NanoSoft.Repository;

namespace NanoSoft.EntityFramework.Identity
{
    public abstract class NanoSoftIdentityDbContext<TIdentityUser> : NanoSoftDbContext
        where TIdentityUser : BaseIdentityUser
    {
        protected NanoSoftIdentityDbContext()
        {

        }

        protected NanoSoftIdentityDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TIdentityUser> Identities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TIdentityUser>(IdentityUserConfigurations<TIdentityUser>.Builder);
        }

        public override EntityValidationState ValidatableSaveChanges()
        {
            var entityValidationState = new IdentityUserConfigurations<TIdentityUser>()
                .IsValid(ChangeTracker.Entries<TIdentityUser>(), this);

            if (!entityValidationState.IsValid)
                return entityValidationState;

            return base.ValidatableSaveChanges();
        }
    }
}
