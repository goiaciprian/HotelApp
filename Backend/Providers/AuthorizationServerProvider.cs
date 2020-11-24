using Backend.Models;
using Backend.Repository;
using Microsoft.Owin.Security.OAuth;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserMasterRepository _repo = new UserMasterRepository();
            {
                User user = await _repo.ValidateUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.isAdmin == 0 ? "User": "Admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.fullName));
                identity.AddClaim(new Claim("Email", user.email));
                identity.AddClaim(new Claim("Id", user.id.ToString()));
                context.Validated(identity);
            }
        }
    }
}