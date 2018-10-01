using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.Identity
{
    public interface IIdentityService<TIdentityResult> : IDisposable
    {
        Task<Response<List<KeyValuePair>>> GetAllAsync();
        Task<Response<TIdentityResult>> LoginAsync(string name, string password);
        Task<Response<string>> GetIdentityNameByIdAsync(Guid userId);
        Task<Response> TryUpdateIdentityNameAsync(Guid userId, string name, string password);
        Task<Response<string>> CreateIdentityAsync(Guid userId, string name);
        Task<Response> DeleteIdentityAsync(Guid userId);
    }
}
