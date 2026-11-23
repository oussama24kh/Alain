using ALAIN_Business.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [AllowAnonymous]
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest();

            TokenResponse? token = ALAIN_Business.User.Login(request);
            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] RefreshRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest();

            TokenResponse? token = ALAIN_Business.User.Refresh(request.RefreshToken);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
        [HttpPost("logout")]
        public ActionResult Logout([FromBody] RefreshRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest();

            bool result = ALAIN_Business.User.Logout(request.RefreshToken);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}