
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace NanoSoft.Test
{
    [PublicAPI]
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public bool InMemoryDb;
        private readonly string _dbName;

        protected abstract void Migrate(DbContext context);
        protected abstract DbContextOptionsBuilder RegisterProvider(DbContextOptionsBuilder builder, string dbName);
        protected abstract TDbContext Initialize(DbContextOptions options);
        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TestUtility(bool inMemoryDb)
        {
            InMemoryDb = inMemoryDb;
            _dbName = Guid.NewGuid().ToString();

            if (inMemoryDb)
                return;

            BuildDb();
        }

        protected virtual void BuildDb()
        {
            using (var context = NewDbContext())
            {
                context.Database.EnsureDeleted();
                Migrate(context);
            }
        }

        public virtual void Dispose()
        {
            if (!InMemoryDb)
            {
                using (var context = NewDbContext())
                {
                    context.Database.EnsureDeleted();
                }

            }
        }

        public TDbContext NewDbContext()
        {
            var builder = new DbContextOptionsBuilder();

            builder = InMemoryDb
                ? builder.UseInMemoryDatabase($"NanoSoft_{_dbName}")
                : RegisterProvider(builder, _dbName);

            return Initialize(builder.Options);
        }

        public TUnitOfWork NewUnitOfWork()
        {
            var context = NewDbContext();

            return Initialize(context);
        }

        protected abstract TApplication NewApp(TUserInfo userInfo);
    }
}
