using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class ExpressionEditDialog : ComponentBase
{
    [Inject] public required IExpressionRepository ExpressionRepository { get; set; }
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }

    private string? _error = "123";
    private Expression? _expression;
    
    private bool DisableSubmit => _expression is null;

    private async Task OnTextChangedAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        try
        {
            _error = text;
            
            var translation = await TranslationRepository
                .Find(text: text, Language.Reference);

            _expression = translation is null ? Expression.Create() : translation.Expression;
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }

    private async Task OnSaveAsync()
    {
        if (_expression is null)
            return;

        try
        {
            await ExpressionRepository.AddOrUpdate(_expression);
        
            MudDialog.Close(DialogResult.Ok(_expression));
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
}

