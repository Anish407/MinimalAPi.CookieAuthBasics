using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MinimalAPi.AuthBasics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDataProtectionProvider dataProtectionProvider;
        private readonly HttpContext httpContext;

        public AuthController(IDataProtectionProvider dataProtectionProvider
            , IHttpContextAccessor httpContextAccessor)
        {
            this.dataProtectionProvider = dataProtectionProvider;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet("SignInUsingInbuiltAuth")]
        public async Task<IActionResult> SignInCookie()
        {
            var protector = dataProtectionProvider.CreateProtector("cookie1");

            var claims = new List<Claim> { new Claim("name", "anish") };
            var identity= new ClaimsIdentity(claims, "mycookie");
            var principal= new ClaimsPrincipal(identity);

            // when we sign in using cookie auth, it creates a encrypted cookie and 
            // saves it in the browser, this is similar to what is done in the SetCookie method
            await httpContext.SignInAsync("mycookie", principal);

            return Ok();
        }

        [HttpGet("ReadUsingInbuiltAuth")]
        public async Task<IActionResult> ReadUsingInbuiltAuth()
        {
            // When we get the cookie, the app.UseAuthentication middleware will
            // decrypt the cookie and populate the claims principal
            // Similar to the ReadCookie()
            var claims = httpContext.User.Claims;

            return Ok(claims);
        }

        [HttpGet("SetCookie")]
        public async Task<IActionResult> SetCookie()
        {
            // when we set a cookie we cannot set its content as plain text.
            // so we use this api to encrypt/decrypt a cookie
            // this is used internally during the authentication process

            var protector = dataProtectionProvider.CreateProtector("cookie1");
            httpContext.Response.Headers["set-cookie"] = $"name={protector.Protect("Anish")}";

            return Ok();
        }

        [HttpGet("ReadCookie")]
        public async Task<IActionResult> ReadCookie()
        {
            var protector = dataProtectionProvider.CreateProtector("cookie1");
            var cookie = httpContext.Request.Headers.Cookie.FirstOrDefault(i => i.StartsWith("name="));
            var encryptedData = cookie.Split("=").Last();
            var data = protector.Unprotect(encryptedData);
            return Ok(data);
        }

    }
}
