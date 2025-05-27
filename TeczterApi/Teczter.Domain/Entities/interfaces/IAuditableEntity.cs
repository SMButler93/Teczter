namespace Teczter.Domain.Entities.interfaces;

internal interface IAuditableEntity
{
    public DateTime CreatedOn { get; }
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; }
    public int RevisedById { get; set; }
    public byte[] RowVersion { get; set; }
}
