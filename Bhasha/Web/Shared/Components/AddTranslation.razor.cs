using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class AddTranslation : ComponentBase
{
    [Inject] public ILogger<AddTranslation> Logger { get; set; } = null!;
    [Inject] public IAuthoringService AuthoringService { get; set; } = null!;
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

    private void OnFocusLost()
    {
        StateHasChanged();
    }
    
    protected override void OnParametersSet()
    {
        SelectedLevel = Level ?? 1;
        Reference = ReferenceTranslation ?? "";
        
        base.OnParametersSet();
    }

    private async Task OnSubmit()
    {
        try
        {
            var expression = await AuthoringService
                .GetOrCreateExpression(Reference, SelectedLevel);
            
            var translation = Translation
                .Create(expression, Language, Target);

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

