using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; } = new Guid();
    public bool IsDeleted { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Pillars Pillar { get; set; }
    public UserAccessLevels AccessLevel { get; set; }

    public List<TestEntity> AssignedTests = [];
}