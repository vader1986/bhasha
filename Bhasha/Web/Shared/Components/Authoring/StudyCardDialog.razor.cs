using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudExRichTextEditor;

namespace Bhasha.Web.Shared.Components.Authoring;

public partial class StudyCardDialog : ComponentBase
{
    [Inject] public required ILogger<StudyCardDialog> Logger { get; set; }
    [Inject] public required IStudyCardRepository StudyCardRepository { get; set; }
    [Inject] public required IResourcesManager ResourcesManager { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    
    [Parameter] public required string Language { get; set; }
    [Parameter] public required StudyCard StudyCard { get; set; }

    private string _name = string.Empty;
    private string _content = string.Empty;
    private string? _audioId;
    private Exception? _error;
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // ReSharper disable once NotAccessedField.Local
    private MudExRichTextEdit _editor;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    
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
    
    private async Task OnResourceChanged(IBrowserFile? audioFile)
    {
        try
        {
            if (audioFile is null)
                return;

            await ResourcesManager.UploadAudio(audioFile.Name, audioFile.OpenReadStream());

            _audioId = audioFile.Name;
        }
        catch (Exception e)
        {
            _error = e;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
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

