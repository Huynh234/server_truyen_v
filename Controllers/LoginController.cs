using Microsoft.AspNetCore.Mvc;
using server_truyen_v.Services;
using server_truyen_v.Schemas;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;
namespace server_truyen_v.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtTokenService _jwt;
        public LoginController(JwtTokenService jwt)
        {
            _jwt = jwt;
        }
        [HttpPost("login")]
        public IActionResult login([FromBody] formLogin use)
        {
            if (use.userName.Equals("admin") && use.pass.Equals("admin"))
            {
                string token = _jwt.GenerateToken(use.userName);
                return Ok(new { Token = token });
            }
            return BadRequest(new { StatusCode = 400 });
        }
    }
}