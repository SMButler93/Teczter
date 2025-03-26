using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Tests.TestStepEntityTests;

[TestFixture]
public class TestStepEntityTests
{
    public static TestStepEntity GetSubjectUnderTest()
    {
        return TestStepInstanceProvider.GetBasicTestStepInstance();
    }

    [Test]
    public void AddLinkUrl_WhenNotValid_ShouldThrow()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var invalidUrl = "invalidUrl.www";

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => sut.AddLinkUrl(invalidUrl));
    }

    [Test]
    public void AddLinkUrl_WhenValid_ShouldNotThrow()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var validUrl = "www.validUrl.com";

        //Act & Assert:
        Should.NotThrow(() => sut.AddLinkUrl(validUrl));
    }

    [Test]
    public void RemoveLinkUrl_WhenRemoved_ShouldNotBePresentInCollection()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var url = "www.Url.com";
        var currentRevisedOn = sut.RevisedOn;
        sut.AddLinkUrl(url);

        //Act:
        sut.RemoveLinkUrl(url);

        //Assert:
        sut.Urls.ShouldNotContain(url);
        sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldBeUpdatedToReflectDeletion()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var currentrevisedOn = sut.RevisedOn;

        //Act:
        sut.Delete();

        //Assert:
        sut.IsDeleted.ShouldBeTrue();
        sut.RevisedOn.ShouldNotBe(currentrevisedOn);
    }

    [Test]
    public void Update_WhenUpdated_ShouldReflectNewValues()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var newStepPlacement = 5;
        var newInstructions = "new instructions.";
        var url = "www.url.com";

        //Act:
        sut.Update(newStepPlacement, newInstructions, [url]);

        //Assert:
        sut.StepPlacement.ShouldBe(newStepPlacement);
        sut.Instructions.ShouldBe(newInstructions);
        sut.Urls.Count.ShouldBe(1);
        sut.Urls.ShouldContain(url);
    }
}
