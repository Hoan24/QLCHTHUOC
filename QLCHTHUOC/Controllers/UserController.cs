using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                    if (!identityResult.Succeeded)
                    {
                        return BadRequest("Failed to assign roles to the user.");
                    }
                }
                return Ok("Registration successful! You can now login.");
            }
            return BadRequest("Failed to register user.");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
                if (user != null)
                {
                    var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                    if (checkPasswordResult)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
                            var response = new LoginResponseDTO
                            {
                                JwtToken = jwtToken
                            };
                            return Ok(response);
                        }
                    }
                }
                return BadRequest("Incorrect username or password.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to login.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
