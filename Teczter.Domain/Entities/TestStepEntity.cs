using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class TestStepEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; private set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<string> Urls { get; set; } = [];

    public void AddLinkUrl(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var _))
        {
            Urls.Add(url);
        }
    }

    public void RemoveLinkUrl(string linkUrl) => Urls.Remove(linkUrl);

    public void Delete() => IsDeleted = true;
}
