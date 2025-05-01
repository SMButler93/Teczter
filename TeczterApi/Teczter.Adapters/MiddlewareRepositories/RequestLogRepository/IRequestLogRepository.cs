using Teczter.Data.MiddlewareModels;

namespace Teczter.Adapters.MiddlewareRepositories.RequestLogRepository;

public interface IRequestLogRepository
{
    public Task LogRequest(RequestLog log);
}