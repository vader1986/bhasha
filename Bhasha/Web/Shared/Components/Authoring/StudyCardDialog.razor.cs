using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExRichTextEditor;

namespace Bhasha.Web.Shared.Components.Authoring;

public partial class StudyCardDialog : ComponentBase
{
    [Inject] public required ILogger<StudyCardDialog> Logger { get; set; }
    [Inject] public required IStudyCardRepository StudyCardRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    
    [Parameter] public required string Language { get; set; }
    [Parameter] public required StudyCard StudyCard { get; set; }

    private string _name = string.Empty;
    private string _content = string.Empty;
    private string? _audioId;
    private Exception? _error;
    private MudExRichTextEdit _editor;
    
    private bool DisableSubmit => string.IsNullOrWhiteSpace(_name) || string.IsNullOrWhiteSpace(_content);

    protected override void OnParametersSet()
    {
        _name = StudyCard.Name;
        _content = StudyCard.Content;
        _audioId = StudyCard.AudioId;
        
        base.OnParametersSet();
    }

    private async Task ValueChanged()
    {
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task OnSubmit()
    {
        try
        {
            var studyCard = StudyCard with
            {
                AudioId = _audioId,
                Name = _name,
                Content = _content
            };

            await StudyCardRepository
                .AddOrUpdate(studyCard);

            MudDialog.Close(DialogResult.Ok(studyCard));
        }
        catch (Exception e)
        {
            _error = e;
            Logger.LogError(e, "failed to submit translation");
            MudDialog.Cancel();
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }
}

