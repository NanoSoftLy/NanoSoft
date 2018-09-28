using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoSoft.Identity;

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
    }
}
