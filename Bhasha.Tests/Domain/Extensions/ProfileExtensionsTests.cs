﻿using System.Linq;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Extensions;
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
        Assert.Equal(expectedResult, result.CurrentChapter);
    }

    [Theory, AutoData]
    public void GivenProfileWithoutCurrentChapter_WhenSubmitCorrectResult_ThenReturnUnchangedProfile(Profile profile)
    {
        // setup
        profile = profile with { CurrentChapter = null };

        // act
        var result = profile.Submit(ValidationResult.Correct);

        // verify
        Assert.Equal(profile, result);
    }

    [Theory, AutoData]
    public void GivenProfile_WhenSubmitCorrectResultWhichCompletesChapter_ThenProfileWithEmptyCurrentChapter(Profile profile)
    {
        // setup
        var completedChapters = new[] { 1 };
        var currentChapter = new ChapterSelection(2, 0, new[]
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
        Assert.Equal(new[] { completedChapters[0], currentChapter.ChapterId }, result.CompletedChapters);
        Assert.Null(result.CurrentChapter);
    }

    [Theory, AutoData]
    public void GivenProfile_WhenSubmitCorrectResult_ThenProfileWithNextPageIndex(Profile profile)
    {
        // setup
        var completedChapters = new[] { 1 };
        var currentChapter = new ChapterSelection(2, 0, new[]
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
        Assert.Equal(completedChapters, result.CompletedChapters);
        Assert.NotNull(result.CurrentChapter);
        Assert.Equal(2, result.CurrentChapter!.ChapterId);
        Assert.Equal(2, result.CurrentChapter!.PageIndex);
        Assert.Equal(new[]
        {
            ValidationResult.Correct,
            ValidationResult.Correct,
            ValidationResult.Wrong
        }, result.CurrentChapter!.Pages);
    }
}

