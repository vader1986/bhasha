using Bhasha.Shared.Domain;

namespace Bhasha.Domain.Extensions;

public static class ProfileExtensions
{
    public static Profile Select(this Profile profile, DisplayedChapter chapter)
    {
        var defaultPageIndex = 0;
        var defaultPages = Enumerable
            .Range(0, chapter.Pages.Length)
            .Select(_ => ValidationResult.Wrong).ToArray();

        return profile with
        {
            CurrentChapter = new ChapterSelection(chapter.Id, defaultPageIndex, defaultPages)
        };
    }

    public static Profile Submit(this Profile profile, ValidationResult result)
    {
        if (profile.CurrentChapter is null)
            return profile;

        var chapter = profile.CurrentChapter;

        chapter.Pages[chapter.PageIndex] = result;

        var nextPageIndex = profile.CurrentChapter.GetNextPageIndex();

        if (nextPageIndex is null)
        {
            var completedChapters = profile.CompletedChapters
                    .Append(chapter.ChapterId)
                    .Distinct().ToArray();

            return profile with
            {
                CompletedChapters = completedChapters,
                CurrentChapter = null,
                Level = completedChapters.Length / 5 + 1
            };
        }

        return profile with
        {
            CurrentChapter = chapter with { PageIndex = nextPageIndex.Value }
        };
    }
}

