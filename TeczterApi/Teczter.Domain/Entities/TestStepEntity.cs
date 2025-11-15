using System.Text.RegularExpressions;
using Teczter.Domain.Entities.interfaces;

namespace Teczter.Domain.Entities;

public class TestStepEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public int TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<string> Urls { get; set; } = [];
    public byte[] RowVersion { get; set; } = [];

    public TeczterValidationResult<TestStepEntity> AddLinkUrl(string url)
    {
        if (!IsValidUrl(url))
        {
            return TeczterValidationResult<TestStepEntity>.Fail($"{url} is an invalid URL.");
        }

        Urls.Add(url);
        RevisedOn = DateTime.Now;
        //RevisedBy

        return TeczterValidationResult<TestStepEntity>.Succeed(this);
    }

    public TeczterValidationResult<TestStepEntity> RemoveLinkUrl(string url)
    {
        if (!Urls.Contains(url, StringComparer.OrdinalIgnoreCase))
        {
            return TeczterValidationResult<TestStepEntity>.Fail("Cannot remove a link that does not belong to this test step");
        }

        Urls.Remove(url);
        RevisedOn = DateTime.Now;
        //RevisedById?

        return TeczterValidationResult<TestStepEntity>.Succeed(this);
    }

    public void Delete()
    {
        //Set revisedBy User Id
        IsDeleted = true;
        RevisedOn = DateTime.Now;
    }

    public void Update(int? stepPlacement, string? instructions, List<string> urls)
    {
        StepPlacement = stepPlacement ?? StepPlacement;
        Instructions = instructions  ?? Instructions;
        Urls.AddRange(urls);
        RevisedOn = DateTime.Now;
        //RevisedBy? 
    }

    private static bool IsValidUrl(string url)
    {
        var pattern = @"^(https?:\/\/www\.|www\.)[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|co\.uk|org)$";

        var regex = new Regex(pattern);

        return regex.IsMatch(url);
    }
}
