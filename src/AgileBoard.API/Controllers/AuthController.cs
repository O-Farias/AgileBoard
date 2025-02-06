using Microsoft.AspNetCore.Mvc;
using AgileBoard.API.Models;
using AgileBoard.API.Services;

namespace AgileBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                // TODO: Implementar verificação de senha com hash
                if (user.PasswordHash != request.Password)
                {
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                var token = _tokenService.GenerateToken(user);

                return Ok(new LoginResponse
                {
                    Token = token,
                    Name = user.Name,
                    Email = user.Email
                });
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized(new { message = "Email ou senha inválidos" });
            }
        }
    }
}