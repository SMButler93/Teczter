﻿using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class UserAdapter(TeczterDbContext dbContext) : IUserAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task<UserEntity?> GetUserByUsername(string username)
    {
        return await _dbContext.Users
            .Where(x => x.Username == username)
            .SingleOrDefaultAsync();
    }
}
