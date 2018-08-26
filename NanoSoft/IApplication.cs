using System;

namespace NanoSoft
{
    public interface IApplication<out TUserInfo, out TSettings, out TCompanyInfo> : IDisposable
        where TUserInfo : UserInfoBase
    {
        TUserInfo User { get; }
        TSettings Settings { get; }
        TCompanyInfo CompanyInfo { get; }
    }
}
