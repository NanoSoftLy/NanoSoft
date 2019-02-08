using System;

namespace NanoSoft
{
    public abstract class Business<TDomain, TUnitOfWork, TUserInfo, TSettings, TCompanyInfo, TRequest>
        where TUnitOfWork : IDisposable
        where TDomain : class
        where TUserInfo : IUser
        where TRequest : Request<TDomain, TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
    {
        protected Business(TUnitOfWork unitOfWork, TUserInfo user, IValidator modelState, TSettings settings, TCompanyInfo companyInfo)
        {
            User = user;
            ModelState = modelState;
            Settings = settings;
            CompanyInfo = companyInfo;
            UnitOfWork = unitOfWork;
        }

        public abstract TRequest Request { get; }

        public TUserInfo User { get; }

        public IValidator ModelState { get; }

        public TSettings Settings { get; }

        public TCompanyInfo CompanyInfo { get; }

        public TUnitOfWork UnitOfWork { get; }
    }
}
