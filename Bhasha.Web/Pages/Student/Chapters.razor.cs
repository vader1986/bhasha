﻿using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Pages.Student
{
	public partial class Chapters : UserPage
	{
        [Inject] public IProgressManager ProgressManager { get; set; } = default!;
		[Inject] public IChapterProvider ChapterProvider { get; set; } = default!;
		[Inject] public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public Guid ProfileId { get; set; }

        private ChapterDescription[] _chapters = Array.Empty<ChapterDescription>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _chapters = await ChapterProvider.GetChapters(ProfileId);
        }

        internal async Task OnSelectChapter(ChapterDescription chapter)
        {
            await ProgressManager.StartChapter(ProfileId, chapter.ChapterId);
            NavigationManager.NavigateTo($"chapter/{chapter.ChapterId}");
        }
    }
}

