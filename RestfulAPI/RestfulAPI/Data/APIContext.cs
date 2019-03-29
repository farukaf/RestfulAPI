using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestfulAPI.Model;

namespace RestfulAPI.Data
{
    /// <summary>
    /// DBContext
    /// </summary>
    public class APIContext : IdentityDbContext<IdentityUser>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Roles
            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );

        }

        public DbSet<RestfulAPI.Model.UserModel> UserModel { get; set; }
        
    }
}
