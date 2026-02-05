using Microsoft.AspNetCore.Identity;

namespace Teczter.Infrastructure.Auth;

public sealed class TeczterUser: IdentityUser
{
    public Guid CompanyId { get; set; }
    public required string CompanyName { get; set; }
}