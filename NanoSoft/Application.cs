using System;

namespace NanoSoft
{
    public abstract class Application<TUnitOfWork, TUserInfo, TIdentityService, TSettings, TCompanyInfo>
        : IApplication<TUserInfo, TSettings, TCompanyInfo>
        where TUnitOfWork : IDisposable
        where TUserInfo : IUser
    {
        protected Application(TUnitOfWork unitOfWork, TIdentityService identityService, TUserInfo user, IValidator modelState, TSettings settings, TCompanyInfo companyInfo)
        {
            User = user;
            ModelState = modelState;
            IdentityService = identityService;
            Settings = settings;
            CompanyInfo = companyInfo;
            UnitOfWork = unitOfWork;
        }

        public TUserInfo User { get; }

        public TIdentityService IdentityService { get; }

        public TSettings Settings { get; }

        public TCompanyInfo CompanyInfo { get; }

        public TUnitOfWork UnitOfWork { get; }

        public IValidator ModelState { get; }

        public virtual void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
