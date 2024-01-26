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
    [Parameter] public string Language { get; set; } = Domain.Language.Bengali;
    [Parameter] public int? Level { get; set; }

    private bool DisableLevel => Level != null;
    private bool DisableSubmit => string.IsNullOrWhiteSpace(_target) || string.IsNullOrWhiteSpace(_reference);

    private Exception? _error;
    private int _level = 1;
    private string? _target;
    private string? _reference;
    
    protected override void OnParametersSet()
    {
        _level = Level ?? 1;
        
        base.OnParametersSet();
    }

    private async Task OnSubmit()
    {
        if (_target is null || _reference is null)
            return;

        try
        {
            var expression = await AuthoringService
                .GetOrCreateExpression(_reference, _level);
            
            var translation = Translation
                .Create(expression, Language, _target);

            await AuthoringService
                .AddOrUpdateTranslation(translation);

            MudDialog.Close(DialogResult.Ok(translation));
        }
        catch (Exception e)
        {
            _error = e;
            Logger.LogError(e, "failed to submit translation");
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }
}

