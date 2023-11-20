using Bhasha.Shared.Domain;

namespace Bhasha.Domain.Extensions;

public static class ChapterSelectionExtensions
{
    public static int? GetNextPageIndex(this ChapterSelection selection)
    {
        var pages = selection.Pages.Length;

        for (var i = 0; i < pages; i++)
        {
            var index = (selection.PageIndex + 1 + i) % pages;
            if (selection.Pages[index] != ValidationResult.Correct)
            {
                return index;
            }
        }

        return null;
    }
}

