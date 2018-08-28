
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace NanoSoft.Test
{
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public bool InMemoryDb { get; protected set; } = true;
        public string ConnectionString { get; protected set; }
        private readonly string _dbName;

        protected abstract void Migrate(DbContext context);
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
                ? builder.UseInMemoryDatabase($"NanoSoft_{dbName}")
                : builder.UseSqlServer(ConnectionString);

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
