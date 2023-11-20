using System;
using Bhasha.Domain;
using Bhasha.Domain.Extensions;
using Bhasha.Shared.Domain;
using FluentAssertions;
using Xunit;

namespace Bhasha.Tests.Domain.Extensions;

public class ChapterSelectionExtensionsTests
{
    [Fact]
    public void GivenChapterSelection_WhenGetNextPageIndexForFinishedChapter_ThenReturnNull()
    {
        // setup
        var selection = new ChapterSelection(Guid.Empty, 0, new[]
        {
            ValidationResult.Correct,
            ValidationResult.Correct,
            ValidationResult.Correct
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        result.Should().BeNull();
    }

    [Fact]
    public void GivenChapterSelect_WhenGetNextPageIndex_ThenReturnNextIndexWithWrongResult()
    {
        // setup
        var selection = new ChapterSelection(Guid.Empty, 1, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Wrong
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        result.Should().Be(3);
    }

    [Fact]
    public void GivenChapterSelect_WhenGetNextPageIndexStartingFromEnd_ThenReturnNextIndexWithWrongResult()
    {
        // setup
        var selection = new ChapterSelection(Guid.Empty, 1, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Correct
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        result.Should().Be(0);
    }
}

