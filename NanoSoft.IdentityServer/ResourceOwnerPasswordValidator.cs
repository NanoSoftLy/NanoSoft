using IdentityServer4.Models;
using IdentityServer4.Validation;
using NanoSoft.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NanoSoft.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IdentityService _service;

        public ResourceOwnerPasswordValidator(IdentityService service)
        {
            _service = service;
        }

        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var response = await _service.LoginAsync(new LoginModel()
                {
                    Name = context.UserName,
                    Password = context.Password
                });

                switch (response.State)
                {
                    case ResponseState.Valid:
                        context.Result = new GrantValidationResult(
                            subject: response.Model.UserId.ToString(),
                            authenticationMethod: "custom",
                            claims: GetUserClaims(response.Model));
                        return;

                    default:
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, response.Message);
                        return;
                }
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(IdentityUser result)
        {
            return new Claim[]
            {
                new Claim(ClaimTypes.Name, result.Name),
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            };
        }
    }
}
