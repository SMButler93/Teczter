using Teczter.Domain.Entities;
using Teczter.WebApi.DTOs.Request;

namespace Teczter.WebApi.Mappers;

public static class DtoMapper
{
    public static TestEntity MapToEntity(CreateTestRequestDto request)
    {
        return new TestEntity
        {
            Title = request.Title,
            Description = request.Description,
            OwningPillar = request.Pillar
        };
    }
}
