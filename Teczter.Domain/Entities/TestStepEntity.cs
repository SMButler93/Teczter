﻿using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

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
    public List<LinkUrl> LinkUrls { get; set; } = [];

    public TestEntity Test { get; set; } = null!;

    public void AddLinkUrl(LinkUrl linkUrl) => LinkUrls.Add(linkUrl);

    public void RemoveLinkUrl(LinkUrl linkUrl) => LinkUrls.Remove(linkUrl);

    public void Delete() => IsDeleted = true;
}
