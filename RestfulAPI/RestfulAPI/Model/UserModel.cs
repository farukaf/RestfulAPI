using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestfulAPI.Model
{
    /// <summary>
    /// Custom IdentityUser hierarchy so can add new columns to user table
    /// </summary>
    public class UserModel : IdentityUser
    {
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
