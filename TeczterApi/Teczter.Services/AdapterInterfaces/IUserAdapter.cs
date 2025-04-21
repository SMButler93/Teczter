using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IUserAdapter
{
    Task<UserEntity?> GetUserByUsername(string username);
}
