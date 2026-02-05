using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Teczter.Infrastructure.Auth;

public class UserDbContext(DbContextOptions<UserDbContext> options): IdentityDbContext<TeczterUser>(options)
{
    
}