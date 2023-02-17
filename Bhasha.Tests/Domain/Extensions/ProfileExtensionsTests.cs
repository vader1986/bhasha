using System;
using System.Linq;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Extensions;
using FluentAssertions;
using Xunit;

namespace Bhasha.Tests.Domain.Extensions;

public class ProfileExtensionsTests
{
    [Theory, AutoData]
    public void GivenProfile_WhenSelectDisplayedChapter_ThenReturnUpdatedProfile(Profile profile, DisplayedChapter chapter)
    {
        // setup
        var expectedResult = new ChapterSelection(chapter.Id, 0, chapter.Pages.Select(_ => ValidationResult.Wrong).ToArray());

        // act
        var result = profile.Select(chapter);

        // verify
        result.CurrentChapter.Should().NotBeNull();
        result.CurrentChapter.Should().Be(expectedResult);
    }

    [Theory, AutoData]
    public void GivenProfileWithoutCurrentChapter_WhenSubmitCorrectResult_ThenReturnUnchangedProfile(Profile profile)
    {
        // setup
        profile = profile with { CurrentChapter = null };

        // act
        var result = profile.Submit(ValidationResult.Correct);

        // verify
        result.Should().Be(profile);
    }

    [Theory, AutoData]
    public void GivenProfile_WhenSubmitCorrectResultWhichCompletesChapter_ThenProfileWithEmptyCurrentChapter(Profile profile)
    {
        // setup
        var completedChapters = new[] { Guid.NewGuid() };
        var currentChapter = new ChapterSelection(Guid.NewGuid(), 0, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Correct
        });

        profile = profile with
        {
            CompletedChapters = completedChapters,
            CurrentChapter = currentChapter
        };

        // act
        var result = profile.Submit(ValidationResult.Correct);

        // verify
        result.CompletedChapters.Should().BeEquivalentTo(new[]
        {
            completedChapters[0],
            currentChapter.ChapterId
        });

        result.CurrentChapter.Should().BeNull();
    }

    [Theory, AutoData]
    public void GivenProfile_WhenSubmitCorrectResult_ThenProfileWithNextPageIndex(Profile profile)
    {
        // setup
        var completedChapters = new[] { Guid.NewGuid() };
        var currentChapter = new ChapterSelection(Guid.NewGuid(), 0, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Wrong
        });

        profile = profile with
        {
            CompletedChapters = completedChapters,
            CurrentChapter = currentChapter
        };

        // act
        var result = profile.Submit(ValidationResult.Correct);

        // verify
        result.CompletedChapters.Should().BeEquivalentTo(completedChapters);
        result.CurrentChapter.Should().NotBeNull();
        result.CurrentChapter!.PageIndex.Should().Be(2);
        result.CurrentChapter!.Pages.Should().BeEquivalentTo(new[]
        {
            ValidationResult.Correct,
            ValidationResult.Correct,
            ValidationResult.Wrong
        });
    }
}

