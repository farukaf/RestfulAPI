using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestfulAPI.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestfulAPI.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<UserModel> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// domain/register
        /// Insert new User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] UserModel model)
        {
            model.SecurityStamp = Guid.NewGuid().ToString();

            var password = model.Password;
            //To remove password from table... comment to save password on database
            model.Password = null;

            var result = await _userManager.CreateAsync(model, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(model, "Customer");
            }
            else
            {
                return BadRequest(result);
            }

            return Ok(model);
        }

        /// <summary>
        /// Check Access and returns token and expiration date
        /// </summary>
        /// <param name="model"></param>
        /// <returns>token and expiration date</returns>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                };
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Security:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Security:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Security:Site"],
                  audience: _configuration["Security:Site"],
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo
                  });
            }
            return Unauthorized();
        }
    }
}