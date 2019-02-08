using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoSoft.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoSoft.EntityFramework
{
    public class SharedConfigurations<TEntity, TDbContext> : SharedConfigurations
        where TEntity : class
    {
        public static void TrackEntity<T>(EntityTypeBuilder<T> builder)
            where T : class
        {
            builder.Property<DateTime?>(CreatedAt);
            builder.Property<DateTime?>(ModifiedAt);
        }

        [NotNull]
        public virtual EntityValidationState IsValid(IEnumerable<EntityEntry<TEntity>> entityEntries, TDbContext context)
        {
            EntityValidationState validationState = null;

            foreach (var entityEntry in entityEntries.ToList())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        CheckUnchanged(entityEntry, context);
                        break;

                    case EntityState.Added:
                        validationState = ValidToAdd(entityEntry, context);
                        break;

                    case EntityState.Modified:
                        validationState = ValidToModify(entityEntry, context);
                        break;

                    case EntityState.Deleted:
                        validationState = ValidToDelete(entityEntry, context);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(entityEntry.State));
                }

                if (validationState == null)
                    continue;

                if (!validationState.IsValid)
                    return validationState;
            }

            return EntityValidationState.Valid;
        }

        protected virtual void CheckUnchanged(EntityEntry<TEntity> entityEntry, TDbContext context)
        {

        }


        protected virtual EntityValidationState ValidToAdd(EntityEntry<TEntity> entityEntry, TDbContext context)
        {
            var createdAt = entityEntry.Property<DateTime?>(CreatedAt);

            if (createdAt == null)
                return EntityValidationState.Valid;

            createdAt.CurrentValue = DateTime.UtcNow;
            return EntityValidationState.Valid;
        }

        protected virtual EntityValidationState ValidToModify(EntityEntry<TEntity> entityEntry, TDbContext context)
        {
            var modifiedAt = entityEntry.Property<DateTime?>(ModifiedAt);

            if (modifiedAt == null)
                return EntityValidationState.Valid;

            modifiedAt.CurrentValue = DateTime.UtcNow;
            return EntityValidationState.Valid;
        }

        protected virtual EntityValidationState ValidToDelete(EntityEntry<TEntity> entityEntry, TDbContext context)
            => EntityValidationState.Valid;
    }

    public class SharedConfigurations
    {
        public const string DecimalField = "decimal(18, 3)";
        public const string DateField = "Date";
        public const int SmallField = 255;
        public const int MidField = 1000;
        public const int BigField = 4000;
        public const string CreatedAt = "_CreatedAt";
        public const string ModifiedAt = "_ModifiedAt";
    }
}