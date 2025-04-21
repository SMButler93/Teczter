using System.Text.RegularExpressions;
using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Entities;

public class TestStepEntity : IAuditableEntity, IHasIntId, ISoftDeleteable
{
    public int Id { get; set; }
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
        if (!IsValidUrl(url))
        {
            throw new TeczterValidationException($"{url} is an invalid URL.");
        }

        Urls.Add(url);
        RevisedOn = DateTime.Now;
    }

    public void RemoveLinkUrl(string linkUrl)
    {
        Urls.Remove(linkUrl);
        RevisedOn = DateTime.Now;
    }

    public void Delete()
    {
        IsDeleted = true;
        RevisedOn = DateTime.Now;
    }

    public void Update(int? stepPlacement, string? instructions, List<string> urls)
    {
        StepPlacement = stepPlacement ?? StepPlacement;
        Instructions = instructions  ?? Instructions;
        Urls.AddRange(urls);
        RevisedOn = DateTime.Now;
    }

    private static bool IsValidUrl(string url)
    {
        var pattern = @"^(https?:\/\/www\.|www\.)[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|co\.uk|org)$";

        var regex = new Regex(pattern);

        return regex.IsMatch(url);
    }
}
