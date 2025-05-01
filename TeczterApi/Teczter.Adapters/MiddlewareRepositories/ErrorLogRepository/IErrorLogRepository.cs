using Teczter.Data.MiddlewareModels;

namespace Teczter.Adapters.MiddlewareRepositories.ErrorLogRepository;

public interface IErrorLogRepository
{
    public Task LogError(ErrorLog log);
}
