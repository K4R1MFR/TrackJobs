using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackJobs.Data;

namespace TrackJobs.Controllers.APIs
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
#pragma warning disable CS8601 // Possible null reference assignment.
        private string secureKey = Environment.GetEnvironmentVariable("TRACKJOBS_API_SECURE_KEY", EnvironmentVariableTarget.Process);
#pragma warning restore CS8601 // Possible null reference assignment.

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/api/Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest("The username or password is incorrect!");
            }
        }

        [Route("/api/Token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest("The username or password is incorrect!");
            }
        }

        //[Authorize(Roles = "Member, Admin, Demo")]
        [Route("/api/testme")]
        [HttpGet]
        public async Task<IActionResult> TestMe()
        {
            var jwt = Request.Cookies["jwt"];
            if(jwt == null)
            {
                return Unauthorized("You need to login first!");
            }

            try
            {
                JwtSecurityToken token = Verify(jwt);

                var email = token.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Unauthorized();
                }

                var roles = await _userManager.GetRolesAsync(user);

                return Ok("Access Granted!");

            } catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        private dynamic Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secureKey);

            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            return validatedToken;
        }


        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(6)).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey)),
                    SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims));

            var output = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("jwt", output, new CookieOptions
            {
                HttpOnly = true,
            });

            return Ok(new
            {
                userid = user.Id,
                username = user.UserName,
                email = user.Email,
                roles = roles
            });
        }
    }
}
