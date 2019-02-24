namespace NanoSoft.Wpf.Services
{
    public class StoredProperties<TUser, TSettings, TCompanyInfo>
    {
        public TUser User { get; set; }
        public TSettings Settings { get; set; }
        public TCompanyInfo CompanyInfo { get; set; }
        public string Token { get; set; }
    }
}