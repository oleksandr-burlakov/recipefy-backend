using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Entities;

public class AuthContext : IdentityDbContext<User, Role, Guid>
{
    
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
        
    }

    public AuthContext()
    {
        
    }
}