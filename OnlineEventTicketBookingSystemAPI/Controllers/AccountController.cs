using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineEventTicketBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JwtConfiguration> _configuration;
        private readonly JwtConfiguration _jwtConfiguration;
        

        public AccountController(UserManager<ApplicationUser> userManger,
            IOptions<JwtConfiguration> configuration)
        {
            _userManager = userManger;
            _configuration = configuration;
            _jwtConfiguration = configuration.Value;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.Roles is not null)
                        foreach (string role in model.Roles)
                            await _userManager.AddToRoleAsync(user, role);
                    else
                        await _userManager.AddToRoleAsync(user, "user");

                    return CreatedAtAction("Register",
                        new { OK = "User created successfully" });
                }

                return BadRequest(result.Errors);

            }

            return UnprocessableEntity(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {

                    List<Claim> claims = [
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier,user.UserName!),
                        new Claim(ClaimTypes.Email,user.Email!),
                    ];

                    var userRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    SymmetricSecurityKey key =
                        new(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")));

                    SigningCredentials signingCredentials =
                        new(key, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken token = new(
                         issuer: _jwtConfiguration.ValidIssuer,
                         audience: _jwtConfiguration.ValidAudience,
                         claims: claims,
                         signingCredentials: signingCredentials,
                         expires: DateTime.Now.AddMinutes(_jwtConfiguration.Expires)
                    );

                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expires = token.ValidTo
                        }
                    );
                }
                return Unauthorized();
            }

            return UnprocessableEntity(ModelState);
        }

        [HttpGet("profile/{id:guid}")]
        [Authorize]
        public IActionResult Profile(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
