using System;

namespace NanoSoft
{
    public abstract class Business<TApplication, TDomain, TUnitOfWork, TUserInfo, TSettings, TCompanyInfo, TRequest>
        where TUnitOfWork : IDisposable
        where TDomain : class
        where TUserInfo : UserInfoBase
        where TApplication : Application<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
        where TRequest : Request<TApplication, TDomain, TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
    {
        protected Business(TApplication application)
        {
            Application = application;
        }

        public TApplication Application { get; }

        public abstract TRequest Request { get; }

        public TUserInfo User => Application.User;

        public IValidator ModelState => Application.ModelState;

        public TSettings Settings => Application.Settings;

        public TCompanyInfo CompanyInfo => Application.CompanyInfo;

        public TUnitOfWork UnitOfWork => Application.UnitOfWork;
    }
}
