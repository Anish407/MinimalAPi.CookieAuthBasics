using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LearnAuthenticationSchemas.Api.Auth
{
    public class CookieAuthHandler : CookieAuthenticationHandler
    {
        public CookieAuthHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authResult = await base.HandleAuthenticateAsync();

            if (authResult.Succeeded)
            {
                return authResult;
            }

            var claims = new List<Claim>
            {
                new Claim("role", "local")
            };
            var identity = new ClaimsIdentity(claims, "local");
            var principal = new ClaimsPrincipal(identity);
            await Context.SignInAsync("local", principal);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, "local"));
        }

    }
}
