using System;
using System.Threading.Tasks;

namespace NanoSoft.Identity
{
    public interface IClientIdentityService<TIdentityResult> : IDisposable
    {
        Task<Response<TIdentityResult>> LoginAsync(LoginModel model);
    }
}
