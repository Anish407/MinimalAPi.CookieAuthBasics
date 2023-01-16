using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

            //services.AddAuthentication().AddCookie() does a similar thing,
            // it creates a cookie using the data protection apis and sets it in the response header

            // give this a name, the same name should be used to decrpyt the cookie
            var protector = dataProtectionProvider.CreateProtector("cookie1");
            // we set the cookie in the response header
            httpContext.Response.Headers["set-cookie"] = $"name={protector.Protect("Anish")}";

            // 

            return Ok();
        }

        [HttpGet("ReadCookie")]
        public async Task<IActionResult> ReadCookie()
        {
            //But every time we cannot write this code to read the cookie contents,
            //So microsoft has created a
            //middleware that decrypts the cookie and creates an object that will
            //represent the logged in user. look at program.cs, we 
            // have created a middleware
            var protector = dataProtectionProvider.CreateProtector("cookie1");
            var cookie = httpContext.Request.Headers.Cookie.FirstOrDefault(i => i.StartsWith("name="));
            var encryptedData = cookie.Split("=").Last();
            var data = protector.Unprotect(encryptedData);
            return Ok(data);
        }

        [HttpGet("ReadFromClaims")]
        public async Task<IActionResult> ReadFromClaims()
        {
            // Either we write the code to decrypt or we use the middleware that decrpyts the token and 
            // 
            return Ok(httpContext.User);
        }

    }
}
