using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recepify.DLL.Entities;

namespace Recepify.DLL;

public class RecepifyContext : IdentityDbContext<User, Role, Guid>
{

    public RecepifyContext(DbContextOptions<RecepifyContext> options) : base(options)
    {
        
    }

    public RecepifyContext()
    {
        
    }
}