using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.API.Model;

namespace MovieStore.API.Controllers
{
    public class AccountController : ControllerBase
    {
        protected readonly JwtSettings jwtSettings;

        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        private IEnumerable<Users> logins = new List<Users>() {
            new Users()
            {
                Id = Guid.NewGuid(),
                EmailId = "adminakp@gmail.com",
                UserName = "Admin",
                Password = "Admin",
            },
            new Users() {
                Id = Guid.NewGuid(),
                    EmailId = "adminakp@gmail.com",
                    UserName = "User1",
                    Password = "Admin",
            }
        };

        [HttpPost("GetToken")]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id,
                    }, jwtSettings);
                }
                else
                {
                    return BadRequest($"wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
