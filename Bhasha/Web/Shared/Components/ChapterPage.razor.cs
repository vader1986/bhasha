using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterPage : ComponentBase
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    
    [Parameter] public required DisplayedChapter Chapter { get; set; }
    [Parameter] public required DisplayedPage Page { get; set; }

    [Parameter] public required Profile Value { get; set; }
    [Parameter] public EventCallback<Profile> ValueChanged { get; set; }
    
    private Translation? _selection;
    
    private int ChapterProgress
    {
        get
        {
            var totalPages = Chapter.Pages.Length * 3;
            
            var correctAnswersPerPage = Value.CurrentChapter?.CorrectAnswers ?? [];
            var correctAnswers = correctAnswersPerPage.Sum(x => Math.Min(3, (int)x));
            
            return (int)Math.Round(100 * (double)correctAnswers / totalPages);
        }
    }
    
    private async Task OnValueChanged()
    {
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateProfileAsync()
    {
        var profile = await StudyingService.GetProfile(Value.Key);

        if (Value != profile)
        {
            await ValueChanged.InvokeAsync(profile);
        }
    }
}

