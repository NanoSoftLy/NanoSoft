
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace NanoSoft.Test
{
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public bool _inMemoryDb;
        private readonly string _dbName;

        protected abstract void Migrate(DbContext context);
        protected abstract DbContextOptionsBuilder RegisterProvider(DbContextOptionsBuilder builder);
        protected abstract TDbContext Initialize(DbContextOptions options);
        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TestUtility(bool inMemoryDb, [CallerMemberName] string dbName = null)
        {
            _inMemoryDb = inMemoryDb;
            _dbName = dbName;

            if (inMemoryDb)
                return;

            var context = NewDbContext(dbName);
            context.Database.EnsureDeleted();
            Migrate(context);
        }

        public void Dispose()
        {
            if (!_inMemoryDb)
                NewDbContext(_dbName).Database.EnsureDeleted();
        }

        public TDbContext NewDbContext([CallerMemberName] string dbName = null)
        {
            Check.NotEmpty(dbName, nameof(dbName));

            var builder = new DbContextOptionsBuilder();

            builder = _inMemoryDb
                ? builder.UseInMemoryDatabase($"NanoSoft_{dbName}")
                : RegisterProvider(builder);

            return Initialize(builder.Options);
        }

        public TUnitOfWork NewUnitOfWork([CallerMemberName] string dbName = null)
        {
            var context = NewDbContext(dbName);

            return Initialize(context);
        }

        protected abstract TApplication NewApp(TUserInfo userInfo, [CallerMemberName] string dbName = null);
    }
}
