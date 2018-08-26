namespace NanoSoft
{
    public class ApplicationOptions<TUnitOfWork, TUserInfo, TSettings, TCompanyInfo>
    {
        protected internal TUnitOfWork UnitOfWork { get; set; }
        protected internal TUserInfo User { get; set; }
        protected internal TSettings Settings { get; set; }
        protected internal TCompanyInfo CompanyInfo { get; set; }
        protected internal IValidator Validator { get; set; }
    }
}
