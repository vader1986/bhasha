﻿using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterPage : ComponentBase
{
    [Parameter] public required DisplayedChapter Chapter { get; set; }
    [Parameter] public required DisplayedPage Page { get; set; }
    [Parameter] public required Bhasha.Domain.ChapterSelection Selection { get; set; }
    [Parameter] public required Func<Translation, Task> Submit { get; set; }

    private int ChapterProgress
    {
        get
        {
            var totalPages = Chapter.Pages.Length * 3;
            var correctResults = Selection.CorrectAnswers.Sum(x => Math.Min(3, (int)x));
            
            return (int)Math.Round(100 * (double)correctResults / totalPages);
        }
    }
    
    internal DisplayedPage<T> GetPage<T>()
    {
        return (DisplayedPage<T>)Page;
    }
}

