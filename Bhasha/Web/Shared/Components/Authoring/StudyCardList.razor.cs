using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Authoring;

public partial class StudyCardList : ComponentBase
{
    [Inject] public required IStudyCardRepository StudyCardRepository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }

    private Language _language = Language.Reference;
    private StudyCard[] _studyCards = [];

    protected override async Task OnParametersSetAsync()
    {
        var studyCards = await StudyCardRepository
            .FindByLanguage(_language);

        _studyCards = studyCards.ToArray();
        
        await base.OnParametersSetAsync();
    }
    
    private async Task OpenEditViewAsync(StudyCard studyCard)
    {
        var parameters = new DialogParameters
        {
            { "StudyCard", studyCard }
        };
        
        await DialogService
            .ShowAsync<StudyCardDialog>("Study Card", parameters);
    }

    private async Task OpenCreateViewAsync()
    {
        var parameters = new DialogParameters
        {
            { "StudyCard", new StudyCard(
                Id: 0,
                Language: _language,
                Name: "New study card",
                Content: "",
                AudioId: null) }
        };
        
        await DialogService
            .ShowAsync<StudyCardDialog>("Study Card", parameters);
    }
}