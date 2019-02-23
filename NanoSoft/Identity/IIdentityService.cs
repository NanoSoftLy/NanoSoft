using System;
using System.Threading.Tasks;

namespace NanoSoft.Identity
{
    public interface IIdentityService<TIdentityResult> : IDisposable
    {
        Task<Response<TIdentityResult>> LoginAsync(LoginModel model);
    }
}
