using System;

namespace NanoSoft
{
    public abstract class Application<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
        : IApplication<TUserInfo, TSettings, TCompanyInfo>
        where TUnitOfWork : IDisposable
        where TUserInfo : IUserInfo
    {
        protected Application(TUnitOfWork unitOfWork, TUserInfo user, IValidator modelState, TSettings settings, TCompanyInfo companyInfo)
        {
            User = user;
            ModelState = modelState;
            Settings = settings;
            CompanyInfo = companyInfo;
            UnitOfWork = unitOfWork;
        }

        public TUserInfo User { get; }

        public TSettings Settings { get; }

        public TCompanyInfo CompanyInfo { get; }

        public TUnitOfWork UnitOfWork { get; }

        public IValidator ModelState { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
