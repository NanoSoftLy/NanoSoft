
using Microsoft.EntityFrameworkCore;
using NanoSoft;
using System;
using System.Runtime.CompilerServices;

namespace FaceAds.Test
{
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public static bool InMemoryDb = true;
        private readonly string _dbName;

        protected abstract void Migrate(DbContext context);
        protected abstract DbContextOptionsBuilder RegisterProvider(DbContextOptionsBuilder builder);
        protected abstract TDbContext Initialize(DbContextOptions options);
        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TestUtility([CallerMemberName] string dbName = null)
        {
            _dbName = dbName;

            if (InMemoryDb)
                return;

            var context = NewDbContext(dbName);
            context.Database.EnsureDeleted();
            Migrate(context);
        }

        public void Dispose()
        {
            if (!InMemoryDb)
                NewDbContext(_dbName).Database.EnsureDeleted();
        }

        public TDbContext NewDbContext([CallerMemberName] string dbName = null)
        {
            Check.NotEmpty(dbName, nameof(dbName));

            var builder = new DbContextOptionsBuilder();

            builder = InMemoryDb
                ? builder.UseInMemoryDatabase($"FaceAdsDb_{dbName}")
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
