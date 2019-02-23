using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.Identity
{
    public interface IIdentityService : IDisposable
    {
        Task<Response<List<KeyValuePair>>> GetAllAsync();
        Task<Response> CreateIdentityAsync(InputModel model);
        Task<Response> UpdateIdentityAsync(Guid id, InputModel model);
        Task<Response> DeleteIdentityAsync(Guid id);
    }
}
