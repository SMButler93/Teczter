﻿using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class UserEntity : IHasGuidid, ISoftDeleteable
{
    private Guid _id;
    public Guid Id => _id;
    public bool IsDeleted { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Department Department { get; set; }
    public UserAccessLevel AccessLevel { get; set; }

    public List<ExecutionEntity> AssignedExcutions = [];
}