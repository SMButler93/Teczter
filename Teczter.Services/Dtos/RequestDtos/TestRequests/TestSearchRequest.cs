using Teczter.Domain.Enums;

namespace Teczter.Services.Dtos.RequestDtos.TestRequests;

public class TestSearchRequest
{
    public string? TestRoundName { get; set; }
    public Pillar? Pillar { get; set; }
    public string? AssignedUserUsername { get; set; }
    public string? TestTitle { get; set; }
    public ExecutionStateType? TestState { get; set; }

    public TestSearchRequest(
        string? testRoundName,
        Pillar? pillar,
        string? assignedUserUsername,
        string? testTitle,
        ExecutionStateType? testState)
    {
        TestRoundName = testRoundName;
        Pillar = pillar;
        AssignedUserUsername = assignedUserUsername;
        TestTitle = testTitle;
        TestState = testState;
    }
}