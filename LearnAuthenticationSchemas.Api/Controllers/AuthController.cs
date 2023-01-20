using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnAuthenticationSchemas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string AuthSchemes =
       "local" + "," +
        "Customer";
        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        //[Authorize(Policy = "onlylocal")]
        [HttpGet("LoginCustomer")]
        public async Task LoginCustomer()
        {
            var claims = new List<Claim>
            { 
                new Claim("role", "customer") 
            };
            var identity = new ClaimsIdentity(claims, "Customer");
            var principal = new ClaimsPrincipal(identity);

            await HttpContextAccessor.HttpContext.SignInAsync("Customer", principal);
        }

        [HttpGet("LoginLocal")]
        public async Task LoginLocal()
        {
            var claims = new List<Claim> 
            {
                new Claim("role", "local") 
            };
            var identity = new ClaimsIdentity(claims, "local");
            var principal = new ClaimsPrincipal(identity);

            await HttpContextAccessor.HttpContext.SignInAsync("local", principal);
        }

        // this endpoint can only be called if the user has the Customer cookie
        [Authorize(Policy = "onlyCustomer")]
        [HttpGet("OnlyCustomers")]
        public async Task<IActionResult> OnlyCustomers()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }

        // this endpoint can only be called if the user has the local cookie
        [Authorize(Policy = "onlylocal")]
        [HttpGet("OnlyLocal")]
        public async Task<IActionResult> OnlyLocal()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }

        // this endpoint can only be called if the user has either local or customer cookie
        [HttpGet("anyone")]
        [Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<IActionResult> anyone()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }
    }
}
