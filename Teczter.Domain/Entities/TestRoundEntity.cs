namespace Teczter.Domain.Entities;

public class TestRoundEntity
{
    public Guid Id { get; } = new Guid();
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public Guid CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public Guid RevisedById { get; set; }
    public string TestRoundName { get; set; } = null!;
    public DateTime? ClosedDate { get; set; } = null;
    public string? TestRoundNotes { get; set; }
    public bool IsComplete => Tests.All(x => x.HasBeenTested);
    public bool IsClosed => ClosedDate.HasValue;

    public List<TestEntity> Tests { get; set; } = [];

    public void AddTest(TestEntity test)
    {
        Tests.Add(test);
    }

    public void DeleteTest(TestEntity test)
    {
        test.IsDeleted = true;
        Tests.Remove(test);
    }

    public void CloseTestRound()
    {
        ClosedDate = DateTime.Now;
    }
}