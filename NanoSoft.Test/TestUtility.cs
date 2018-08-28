
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace NanoSoft.Test
{
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public bool InMemoryDb { get; protected set; } = true;
        private readonly string _dbName;

        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TestUtility([CallerMemberName] string dbName = null)
        {
            _dbName = dbName;

            if (InMemoryDb)
                return;

            var context = NewDbContext(dbName);
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        public void Dispose()
        {
            if (!InMemoryDb)
                NewDbContext(_dbName).Database.EnsureDeleted();
        }

        public abstract TDbContext NewDbContext([CallerMemberName] string dbName = null);

        public TUnitOfWork NewUnitOfWork([CallerMemberName] string dbName = null)
        {
            var context = NewDbContext(dbName);

            return Initialize(context);
        }

        protected abstract TApplication NewApp(TUserInfo userInfo, [CallerMemberName] string dbName = null);
    }
}
