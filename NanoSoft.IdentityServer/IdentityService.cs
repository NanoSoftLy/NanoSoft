using Microsoft.EntityFrameworkCore;
using NanoSoft.Extensions;
using NanoSoft.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NanoSoft.IdentityServer
{
    public class IdentityService : IIdentityService<IdentityUser>
    {
        private readonly IdentityDbContext _identityDbContext;
        public const string Salt = "$2a$11$HFUkCwXa0DjXlj0QaSE9Ne";

        public IdentityService(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task<Response<List<KeyValuePair>>> GetAllAsync()
        {
            var result = await _identityDbContext.Identities
                           .Select(i => new KeyValuePair()
                           {
                               Key = i.UserId,
                               Value = i.Name
                           }).ToListAsync();

            return Response.Success(result);
        }

        public async Task<Response<IdentityUser>> LoginAsync(string name, string password)
        {
            var hashedPassword = password.ToBCryptHash(Salt);

            var identity = await _identityDbContext.Identities
                .FirstOrDefaultAsync(i => i.Name == name && i.Password == hashedPassword);

            if (identity == null)
            {
                Console.WriteLine(@"No identity found");
                return Response.Fail(ResponseState.NotFound);
            }

            return Response.Success(identity);
        }

        public async Task<Response<string>> GetIdentityNameByIdAsync(Guid userId)
        {
            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.UserId == userId);

            if (identity == null)
                return Response.Fail(ResponseState.NotFound);

            return Response.Success<string>(identity.Name);
        }

        public async Task<Response> TryUpdateIdentityNameAsync(Guid userId, string name, string password)
        {
            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.UserId == userId);

            if (identity == null)
                return Response.Fail(ResponseState.NotFound);

            var hashedPassword = password.ToBCryptHash(Salt);

            identity.Name = name;
            identity.Password = hashedPassword;

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
                return Response.Fail(ResponseState.Unacceptable, state.Message);

            return Response.SuccessEdit();
        }

        public async Task<Response<string>> CreateIdentityAsync(Guid userId, string name)
        {
            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.UserId == userId);

            if (identity != null)
                return Response.SuccessCreate(identity.Name);

            var hashedPassword = "12345".ToBCryptHash(Salt);

            identity = new IdentityUser()
            {
                UserId = userId,
                Id = Guid.NewGuid(),
                Name = name,
                Password = hashedPassword
            };

            await _identityDbContext.Identities.AddAsync(identity);

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
            {
                identity.Name = Guid.NewGuid().ToString();
                state = await _identityDbContext.ValidatableSaveChangesAsync();
            }

            if (!state.IsValid)
                throw new Exception("Identity Creation Failed !");

            return Response.SuccessCreate(identity.Name);
        }

        public Task<bool> AnyAsync()
        {
            return _identityDbContext.Identities.AnyAsync();
        }

        public void Dispose()
        {
            _identityDbContext.Dispose();
        }

        public async Task<Response> DeleteIdentityAsync(Guid userId)
        {
            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.UserId == userId);

            if (identity == null)
                return Response.Fail(ResponseState.NotFound);

            _identityDbContext.Identities.Remove(identity);

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
                return Response.Fail(ResponseState.Unacceptable, state.Message);

            return Response.SuccessDelete();
        }
    }
}
