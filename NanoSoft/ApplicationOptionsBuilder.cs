namespace NanoSoft
{
    public class ApplicationOptionsBuilder<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
    {
        private ApplicationOptions<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> Options { get; }

        public ApplicationOptionsBuilder(TUnitOfWork unitOfWork)
        {
            Options = new ApplicationOptions<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>()
            {
                UnitOfWork = unitOfWork
            };
        }

        public ApplicationOptionsBuilder<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> AddUser(TUserInfo user)
        {
            Options.User = user;
            return this;
        }

        public ApplicationOptionsBuilder<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> UseSettings(TSettings settings)
        {
            Options.Settings = settings;
            return this;
        }

        public ApplicationOptionsBuilder<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> UseCustomValidator(IValidator validator)
        {
            Options.Validator = validator;
            return this;
        }

        public ApplicationOptionsBuilder<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> UseCompanyInfo(TCompanyInfo companyInfo)
        {
            Options.CompanyInfo = companyInfo;
            return this;
        }

        public ApplicationOptions<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo> Build()
        {
            if (Options.Validator == null)
                Options.Validator = new ModelState();

            return Options;
        }
    }
}
