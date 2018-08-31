
using Microsoft.EntityFrameworkCore;
using System;

namespace NanoSoft.Test
{
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public bool _inMemoryDb;
        private readonly string _dbName;

        protected abstract void Migrate(DbContext context);
        protected abstract DbContextOptionsBuilder RegisterProvider(DbContextOptionsBuilder builder, string dbName);
        protected abstract TDbContext Initialize(DbContextOptions options);
        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TestUtility(bool inMemoryDb)
        {
            _inMemoryDb = inMemoryDb;
            _dbName = Guid.NewGuid().ToString();

            if (inMemoryDb)
                return;

            var context = NewDbContext();
            context.Database.EnsureDeleted();
            Migrate(context);
        }

        public void Dispose()
        {
            if (!_inMemoryDb)
                NewDbContext().Database.EnsureDeleted();
        }

        public TDbContext NewDbContext()
        {
            var builder = new DbContextOptionsBuilder();

            builder = _inMemoryDb
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
