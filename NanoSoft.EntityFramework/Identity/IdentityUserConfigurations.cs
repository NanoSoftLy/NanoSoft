using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoSoft.Identity;
using NanoSoft.Repository;
using NanoSoft.Resources;
using System.Linq;

namespace NanoSoft.EntityFramework.Identity
{
    public class IdentityUserConfigurations<TIdentityUser> : SharedConfigurations<TIdentityUser, NanoSoftIdentityDbContext<TIdentityUser>>
        where TIdentityUser : BaseIdentityUser
    {
        public static void Builder(EntityTypeBuilder<TIdentityUser> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasMaxLength(SmallField);
            builder.Property(e => e.Password).IsRequired().HasMaxLength(SmallField);
        }


        protected override EntityValidationState ValidToAdd(EntityEntry<TIdentityUser> entityEntry, NanoSoftIdentityDbContext<TIdentityUser> context)
        {
            var entity = entityEntry.Entity;

            if (context.Identities.Any(i => i.Name == entity.Name))
                return EntityValidationState.Error(e => entity.Name, SharedMessages.NameExisted);

            return base.ValidToAdd(entityEntry, context);
        }

        protected override EntityValidationState ValidToModify(EntityEntry<TIdentityUser> entityEntry, NanoSoftIdentityDbContext<TIdentityUser> context)
        {
            var entity = entityEntry.Entity;

            if (context.Identities.Any(i => i.Name == entity.Name && i.Id != entity.Id))
                return EntityValidationState.Error(e => entity.Name, SharedMessages.NameExisted);

            return base.ValidToModify(entityEntry, context);
        }
    }
}
