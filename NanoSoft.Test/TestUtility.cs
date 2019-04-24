
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;

namespace NanoSoft.Test
{
    [PublicAPI]
    public abstract class TestUtility<TApplication, TUserInfo, TUnitOfWork, TDbContext, TIdentityDbContext> : IDisposable
        where TDbContext : DbContext
        where TIdentityDbContext : DbContext
    {
        public virtual string ApplicationUrl => "http://localhost:4001/";
        public virtual string IdentityProviderUrl => "http://localhost:4000/connect/token";

        public virtual string WebTestsConnectionString => "Server=.\\SQLEXPRESS; Database=NANOSOFT_TEST_WEB; Integrated security=true;";
        public virtual string IdentityTestsConnectionString => "Server=.\\SQLEXPRESS; Database=NANOSOFT_TEST_IDENTITY; Integrated security=true;";

        private readonly bool _useIntegrationTesting;


        private static HttpClient _client;
        private static HttpClient Client
        {
            get
            {
                if (_client != null)
                    return _client;

                _client = new HttpClient();
                return _client;
            }
        }


        public string DbName { get; set; }


        public TestUtility(bool useIntegrationTesting)
        {
            _useIntegrationTesting = useIntegrationTesting;
            DbName = Guid.NewGuid().ToString();
            ReBuildDb();
        }

        protected virtual void ReBuildDb()
        {
            if (!_useIntegrationTesting)
                return;

            Destroy();
            BuildDb();
        }

        protected abstract void BuildDb();
        protected abstract void Destroy();

        public virtual void Dispose()
        {
            if (!_useIntegrationTesting)
                return;

            Destroy();
        }

        protected abstract DbContextOptionsBuilder RegisterProvider(DbContextOptionsBuilder builder, string dbName);
        protected abstract TDbContext Initialize(DbContextOptions options);
        protected abstract TIdentityDbContext InitializeIdp(DbContextOptions options);
        protected abstract TUnitOfWork Initialize(TDbContext context);

        public TDbContext NewDbContext()
        {
            var builder = new DbContextOptionsBuilder();

            builder = _useIntegrationTesting
                ? RegisterProvider(builder, WebTestsConnectionString)
                : builder.UseInMemoryDatabase($"NanoSoft_{DbName}");

            return Initialize(builder.Options);
        }

        public virtual TIdentityDbContext NewIdentityDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            var options = _useIntegrationTesting
                ? RegisterProvider(optionsBuilder, IdentityTestsConnectionString)
                    .Options
                : optionsBuilder.UseInMemoryDatabase($"Idp_{DbName}")
                    .Options;

            return InitializeIdp(options);
        }

        public TUnitOfWork NewUnitOfWork()
        {
            var context = NewDbContext();

            return Initialize(context);
        }

        protected abstract TApplication NewApp(TUserInfo userInfo);
    }
}
