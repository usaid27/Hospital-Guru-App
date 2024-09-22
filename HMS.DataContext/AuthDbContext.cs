using HMS.DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.DataContext
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
    }
}
