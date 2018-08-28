using NanoSoft.Repository;
using System;
using System.Threading.Tasks;

namespace NanoSoft
{
    public abstract class Request<TApplication, TDomain, TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
        where TUnitOfWork : IDisposable
        where TDomain : class
        where TUserInfo : IUserInfo
        where TApplication : Application<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
    {
        public Request(TApplication application, IRepository<TDomain> repository, bool includeRelated = false)
        {
            User = application.User;
            UnitOfWork = application.UnitOfWork;
            Application = application;
            Repository = repository;
            _includeRelated = includeRelated;
        }

        private Response _response;
        private readonly bool _includeRelated;

        protected TUserInfo User { get; }
        protected TUnitOfWork UnitOfWork { get; }
        protected TApplication Application { get; }
        protected IRepository<TDomain> Repository { get; }
        public TDomain Result { get; protected set; }

        public Response<TModel> Response<TModel>()
        {
            return new Response<TModel>(_response);
        }

        public Response Response()
        {
            return _response;
        }

        public Task<bool> IsValidAsync(Func<TUserInfo, bool> policy)
        {
            if (policy != null && (User == null || !policy(User)))
            {
                _response = NanoSoft.Response.Fail(User == null
                    ? ResponseState.Unauthorized
                    : ResponseState.Forbidden);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public async Task<bool> IsValidAsync(Func<TUserInfo, TDomain, bool> policy, Guid id)
        {
            if (id == default(Guid))
            {
                _response = NanoSoft.Response.Fail(ResponseState.BadRequest);
                return false;
            }

            Result = await Repository.FindAsync(id, _includeRelated);

            if (Result == null)
            {
                _response = NanoSoft.Response.Fail(ResponseState.NotFound);
                return false;
            }

            if (policy != null && (User == null || !policy(User, Result)))
            {
                _response = NanoSoft.Response.Fail(User == null
                    ? ResponseState.Unauthorized
                    : ResponseState.Forbidden);
                return false;
            }

            return true;
        }

        public Task<bool> IsValidAsync(object model, Func<TUserInfo, bool> policy)
        {
            if (policy != null && (User == null || !policy(User)))
            {
                _response = NanoSoft.Response.Fail(User == null
                    ? ResponseState.Unauthorized
                    : ResponseState.Forbidden);
                return Task.FromResult(false);
            }

            if (!Application.ModelState.IsValid(model))
            {
                _response = NanoSoft.Response.Fail(Application.ModelState);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public async Task<bool> IsValidAsync(object model, Func<TUserInfo, TDomain, bool> policy, Guid id)
        {
            if (id == default(Guid))
            {
                _response = NanoSoft.Response.Fail(ResponseState.BadRequest);
                return false;
            }

            Result = await Repository.FindAsync(id, _includeRelated);

            if (Result == null)
            {
                _response = NanoSoft.Response.Fail(ResponseState.NotFound);
                return false;
            }

            if (policy != null && (User == null || !policy(User, Result)))
            {
                _response = NanoSoft.Response.Fail(User == null
                    ? ResponseState.Unauthorized
                    : ResponseState.Forbidden);
                return false;
            }

            if (!Application.ModelState.IsValid(model))
            {
                _response = NanoSoft.Response.Fail(Application.ModelState);
                return false;
            }

            return true;
        }
    }
}
