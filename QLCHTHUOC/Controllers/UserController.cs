using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ITokenRepository _tokenRepository;
        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<UserController> logger, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _tokenRepository = tokenRepository;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerRequestDTO.Username,
                    Email = registerRequestDTO.Username
                };

                var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
                if (identityResult.Succeeded)
                {
                    // Gán vai trò cho người dùng
                    if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                    {
                        var roleResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                        if (!roleResult.Succeeded)
                        {
                            return BadRequest("Failed to assign roles to the user.");
                        }
                    }
                    _logger.LogInformation($"Đăng ký thành công từ API: {registerRequestDTO.Username}");
                    return Ok("Đăng ký thành công từ API");
                }
                else
                {
                    _logger.LogError("Đăng ký thất bại từ API");
                    return BadRequest("Đăng ký thất bại từ API");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thực hiện đăng ký từ API");
                return StatusCode(500, "Lỗi khi thực hiện đăng ký từ API");
            }
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
                        if (roles != null && roles.Any())
                        {
                            var jwtToken = CreateJWTToken(user, roles.ToList());
                            var response = new LoginResponseDTO
                            {
                                JwtToken = jwtToken,
                                Username = user.UserName,
                                Email = user.Email,
                                Roles = roles.ToList()
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


        private string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}