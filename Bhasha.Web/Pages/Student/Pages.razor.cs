using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Pages.Student;

public partial class Pages : UserPage
{
    [Inject] public ISubmissionManager SubmissionManager { get; set; } = default!;
    [Inject] public IChapterProvider ChapterProvider { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public Guid ProfileId { get; set; }
    [Parameter] public Guid ChapterId { get; set; }
    [Parameter] public int Index { get; set; }

    private DisplayedChapter? _chapter;
    private DisplayedPage? _page;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _chapter = await ChapterProvider.GetChapter(ProfileId, ChapterId);
        _page = _chapter.Pages[Index];
    }

    internal async Task OnSubmit(Translation translation)
    {
        if (_page == null)
            return;

        var expressionId = _page.Word.ExpressionId;
        var submission = new Submission(ProfileId, expressionId, translation);

        var feedback = await SubmissionManager.Accept(submission);
        var profile = feedback.Profile;

        if (profile.ChapterId == null)
        {
            NavigationManager.NavigateTo($"chapters/{ProfileId}");
        }
        else
        {
            NavigationManager.NavigateTo($"pages/{ProfileId}/{ChapterId}/{profile.PageIndex}");
        }
    }
}


