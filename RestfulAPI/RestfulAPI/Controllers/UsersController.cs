using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPI.Controllers
{
    /// <summary>
    /// User CRUD
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly APIContext _context;
        private readonly UserManager<UserModel> _userManager;


        public UsersController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
            //_context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserModel> GetUser()
        {
            return _userManager.Users.ToList(); ;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstAsync(u => u.Id == id);
            if (user != null)
            {
                //setting manually what can be changed
                user.UserName = model.UserName;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.Email = model.Email;
                user.Name = model.Name;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return NotFound();
            }
        }
        
        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserModel model)
        {
            model.SecurityStamp = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(model, model.Password);
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByIdAsync(id);

                var ret = await _userManager.DeleteAsync(user);
                return Ok(ret);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private bool UserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}