using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Tests;

[TestFixture]
public class TestStepEntityTests
{
    private TestStepEntity _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new TestStepEntity
        {
            StepPlacement = 1,
            Instructions = "Step 1"
        };
    }

    [Test]
    public void AddLinkUrl_WhenNotValid_ShouldThrow()
    {
        //Arrange:
        var invalidUrl = "invalidUrl.www";

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => _sut.AddLinkUrl(invalidUrl));
    }

    [Test]
    public void AddLinkUrl_WhenValid_ShouldNotThrow()
    {
        //Arrange:
        var validUrl = "www.validUrl.com";

        //Act & Assert:
        Should.NotThrow(() => _sut.AddLinkUrl(validUrl));
    }

    [Test]
    public void RemoveLinkUrl_WhenRemoved_ShouldNotBePresentInCollection()
    {
        //Arrange:
        var url = "www.Url.com";
        var currentRevisedOn = _sut.RevisedOn;
        _sut.AddLinkUrl(url);

        //Act:
        _sut.RemoveLinkUrl(url);

        //Assert:
        _sut.Urls.ShouldNotContain(url);
        _sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldBeUpdatedToReflectDeletion()
    {
        //Arrange:
        var currentrevisedOn = _sut.RevisedOn;

        //Act:
        _sut.Delete();

        //Assert:
        _sut.IsDeleted.ShouldBeTrue();
        _sut.RevisedOn.ShouldNotBe(currentrevisedOn);
    }

    [Test]
    public void Update_WhenUpdated_ShouldReflectNewValues()
    {
        //Arrange:
        var newStepPlacement = 5;
        var newInstructions = "new instructions.";
        var url = "www.url.com";

        //Act:
        _sut.Update(newStepPlacement, newInstructions, [url]);

        //Assert:
        _sut.StepPlacement.ShouldBe(newStepPlacement);
        _sut.Instructions.ShouldBe(newInstructions);
        _sut.Urls.Count.ShouldBe(1);
        _sut.Urls.ShouldContain(url);
    }
}
