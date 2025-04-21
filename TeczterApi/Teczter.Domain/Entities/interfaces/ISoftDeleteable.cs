namespace Teczter.Domain.Entities.interfaces;

internal interface ISoftDeleteable
{
    public bool IsDeleted { get; set; }
}
