using Microsoft.EntityFrameworkCore;
using NanoSoft.Extensions;
using NanoSoft.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework.Identity
{
    public class IdentityService<TIdentityContext, TIdentityUser> : IIdentityService
        where TIdentityContext : NanoSoftIdentityDbContext<TIdentityUser>
        where TIdentityUser : BaseIdentityUser, new()
    {
        private readonly TIdentityContext _identityDbContext;
        public const string Salt = "$2a$11$HFUkCwXa0DjXlj0QaSE9Ne";

        public IdentityService(TIdentityContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public ModelState ModelState { get; } = new ModelState();

        public async Task<Response<List<KeyValuePair>>> GetAllAsync()
        {
            var result = await _identityDbContext.Identities
                           .Select(i => new KeyValuePair()
                           {
                               Key = i.Id,
                               Value = i.Name
                           }).ToListAsync();

            return Response.Success(result);
        }

        public async Task<Response<TIdentityUser>> LoginAsync(LoginModel model)
        {
            if (!ModelState.IsValid(model))
                return ModelState.GetResponse();

            var hashedPassword = model.Password.ToBCryptHash(Salt);

            var identity = await _identityDbContext.Identities
                .FirstOrDefaultAsync(i => i.Name == model.Name && i.Password == hashedPassword);

            if (identity == null)
            {
                Console.WriteLine(@"No identity found");
                return Response.Fail(ResponseState.NotFound);
            }

            return Response.Success(identity);
        }


        public async Task<Response<BaseIdentityUser>> UpdateIdentityAsync(Guid id, InputModel model)
        {
            if (!ModelState.IsValid(model))
                return ModelState.GetResponse();

            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.Id == id);

            if (identity == null)
                return Response.Fail(ResponseState.NotFound);

            var hashedPassword = model.Password.ToBCryptHash(Salt);

            identity.Name = model.Name;
            identity.Password = hashedPassword;

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
                return Response.AddError(state);

            return Response.SuccessEdit<BaseIdentityUser>(identity);
        }

        public async Task<Response<BaseIdentityUser>> CreateIdentityAsync(InputModel model)
        {
            if (!ModelState.IsValid(model))
                return ModelState.GetResponse();

            var hashedPassword = model.Password.ToBCryptHash(Salt);

            var identity = new TIdentityUser()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Password = hashedPassword
            };

            await _identityDbContext.Identities.AddAsync(identity);

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
                return Response.AddError(state);

            return Response.SuccessCreate<BaseIdentityUser>(identity);
        }

        public Task<bool> AnyAsync()
        {
            return _identityDbContext.Identities.AnyAsync();
        }

        public void Dispose()
        {
            _identityDbContext.Dispose();
        }

        public async Task<Response> DeleteIdentityAsync(Guid id)
        {
            var identity = await _identityDbContext.Identities
                           .FirstOrDefaultAsync(i => i.Id == id);

            if (identity == null)
                return Response.Fail(ResponseState.NotFound);

            _identityDbContext.Identities.Remove(identity);

            var state = await _identityDbContext.ValidatableSaveChangesAsync();

            if (!state.IsValid)
                return Response.AddError(state);

            return Response.SuccessDelete();
        }
    }
}
