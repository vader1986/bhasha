using Bhasha.Domain;
using Bhasha.Domain.Extensions;
using Xunit;

namespace Bhasha.Tests.Domain.Extensions;

public class ChapterSelectionExtensionsTests
{
    [Fact]
    public void GivenChapterSelection_WhenGetNextPageIndexForFinishedChapter_ThenReturnNull()
    {
        // setup
        var selection = new ChapterSelection(default, 0, new[]
        {
            ValidationResult.Correct,
            ValidationResult.Correct,
            ValidationResult.Correct
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        Assert.Null(result);
    }

    [Fact]
    public void GivenChapterSelect_WhenGetNextPageIndex_ThenReturnNextIndexWithWrongResult()
    {
        // setup
        var selection = new ChapterSelection(default, 1, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Wrong
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        Assert.Equal(3, result);
    }

    [Fact]
    public void GivenChapterSelect_WhenGetNextPageIndexStartingFromEnd_ThenReturnNextIndexWithWrongResult()
    {
        // setup
        var selection = new ChapterSelection(default, 1, new[]
        {
            ValidationResult.Wrong,
            ValidationResult.Wrong,
            ValidationResult.Correct,
            ValidationResult.Correct
        });

        // act
        var result = selection.GetNextPageIndex();

        // verify
        Assert.Equal(0, result);
    }
}

