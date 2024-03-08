using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class TranslationDialog : ComponentBase
{
    [Inject] public required ILogger<TranslationDialog> Logger { get; set; }
    [Inject] public required IAuthoringService AuthoringService { get; set; }
    [Inject] public required ITranslator Translator { get; set; }
    
    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }
    
    [Parameter] public required string Language { get; set; }
    [Parameter] public int? Level { get; set; }
    [Parameter] public string? ReferenceTranslation { get; set; }

    private bool DisableLevel => Level != null;
    private bool DisableSubmit => string.IsNullOrWhiteSpace(Target) || string.IsNullOrWhiteSpace(Reference) || Level < 1;
    private bool DisableReference => !string.IsNullOrWhiteSpace(ReferenceTranslation);

    private Exception? _error;
    
    private int SelectedLevel { get; set; } = 1;
    private string Target { get; set; } = "";
    private string Reference { get; set; } = "";
    private string Spoken { get; set; } = "";

    private async Task OnFocusLost()
    {
        if (!string.IsNullOrWhiteSpace(Reference) && string.IsNullOrWhiteSpace(Target))
        {
            await AutoFillTranslation(Reference);
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task AutoFillTranslation(string reference)
    {
        var (translation, spoken) = await Translator.Translate(reference, Language);

        Target = translation;
        Spoken = spoken;
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        SelectedLevel = Level ?? 1;
        Reference = ReferenceTranslation ?? "";

        if (!string.IsNullOrWhiteSpace(ReferenceTranslation))
        {
            await AutoFillTranslation(ReferenceTranslation);
        }
    }

    private async Task OnSubmit()
    {
        try
        {
            var expression = await AuthoringService
                .GetOrCreateExpression(Reference, SelectedLevel);

            var translation = new Translation(
                Id: default,
                Language: Language,
                Text: Target,
                Spoken: string.IsNullOrWhiteSpace(Spoken) ? null : Spoken,
                AudioId: default,
                Expression: expression);

            await AuthoringService
                .AddOrUpdateTranslation(translation);

            MudDialog.Close(DialogResult.Ok(translation));
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

