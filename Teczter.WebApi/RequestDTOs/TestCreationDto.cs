using Teczter.Domain.Entities;

namespace Teczter.WebApi.RequestDTOs
{
    public class TestCreationDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Pillar { get; set; } = null!;

        public TestEntity MapToEntity()
        {
            return new TestEntity
            {
                Title = this.Title,
                Description = this.Description,
                Pillar = this.Pillar
            };
        }
    }
}
