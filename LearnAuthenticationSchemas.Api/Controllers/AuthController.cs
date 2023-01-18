using Microsoft.AspNetCore.Authentication;
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

        [Authorize(Policy = "onlyCustomer")]
        [HttpGet("OnlyCustomers")]
        public async Task<IActionResult> OnlyCustomers()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }

        [Authorize(Policy = "onlylocal")]
        [HttpGet("OnlyLocal")]
        public async Task<IActionResult> OnlyLocal()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }


        [HttpGet("anyone")]
        public async Task<IActionResult> anyone()
        {
            return Ok(HttpContextAccessor.HttpContext.User.Claims);
        }
    }
}
