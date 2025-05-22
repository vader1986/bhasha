using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Authoring;

public partial class StudyCardSelectDialog : ComponentBase
{
    [Inject] public required IStudyCardRepository StudyCardRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    
    [Parameter] public required Language Language { get; set; }
    [Parameter] public required Language StudyLanguage { get; set; }
    
    private StudyCard[] StudyCards { get; set; } = [];
    private StudyCard? SelectedStudyCard { get; set; }
    private bool DisableSubmit => SelectedStudyCard is null;

    protected override async Task OnParametersSetAsync()
    {
        StudyCards = (await StudyCardRepository
            .FindByLanguage(
                language: Language,
                studyLanguage: StudyLanguage))
            .ToArray();
        
        await base
            .OnParametersSetAsync();
    }

    private async Task UpdateViewAsync()
    {
        await InvokeAsync(StateHasChanged);
    }

    private void Submit()
    {
        if (SelectedStudyCard is null)
            return;
        
        MudDialog
            .Close(DialogResult.Ok(SelectedStudyCard));
    }
}