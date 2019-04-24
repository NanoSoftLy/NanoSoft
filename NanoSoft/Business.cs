using System;

namespace NanoSoft
{
    public abstract class Business<TDomain, TUnitOfWork, TUserInfo, TIdentityService, TSettings, TCompanyInfo, TRequest>
        where TUnitOfWork : IDisposable
        where TDomain : class
        where TUserInfo : IUser
        where TRequest : Request<TDomain, TUnitOfWork, TUserInfo>
    {
        protected Business(TUnitOfWork unitOfWork, TUserInfo user, TIdentityService identityService, IValidator modelState, TSettings settings, TCompanyInfo companyInfo)
        {
            User = user;
            ModelState = modelState;
            IdentityService = identityService;
            Settings = settings;
            CompanyInfo = companyInfo;
            UnitOfWork = unitOfWork;
        }

        public abstract TRequest Request { get; }

        public TUserInfo User { get; }

        public TIdentityService IdentityService { get; }

        public IValidator ModelState { get; }

        public TSettings Settings { get; }

        public TCompanyInfo CompanyInfo { get; }

        public TUnitOfWork UnitOfWork { get; }
    }
}
