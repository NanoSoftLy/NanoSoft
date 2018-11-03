using System;

namespace NanoSoft
{
    public abstract class Application<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
        : IApplication<TUserInfo, TSettings, TCompanyInfo>
        where TUnitOfWork : IDisposable
        where TUserInfo : IUserInfo
    {
        protected Application(ApplicationOptions<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> options)
        {
            User = options.User;
            Settings = options.Settings;
            CompanyInfo = options.CompanyInfo;
            UnitOfWork = options.UnitOfWork;
            ModelState = options.Validator;
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
