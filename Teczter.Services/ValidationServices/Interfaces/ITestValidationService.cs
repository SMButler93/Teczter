using Teczter.Domain.Entities;
using Teczter.Services.Validations;

namespace Teczter.Services.ValidationServices.Interfaces
{
    public interface ITestValidationService
    {
        bool HasUniqueTitle(string testTitle);
    }
}
