using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface IUserAdapter
{
    Task<UserEntity?> GetUserByUsername(string username);
}
