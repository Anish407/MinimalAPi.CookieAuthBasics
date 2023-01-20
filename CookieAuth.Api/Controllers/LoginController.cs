using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookieAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
            {
                new Claim("username", "Anish")
            };
            var identity = new ClaimsIdentity(claims, "local");
            var principal = new ClaimsPrincipal(identity);

            await  _httpContextAccessor.HttpContext.SignInAsync("local", principal);

            return Ok();
        }

        [HttpGet("LoginUsingPathCookie")]
        public async Task<IActionResult> LoginUsingPathCookie()
        {
            var claims = new List<Claim>
            {
                new Claim("username", "Anish")
            };
            var identity = new ClaimsIdentity(claims, "path");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync("path", principal);

            return Ok();
        }
    }
}
