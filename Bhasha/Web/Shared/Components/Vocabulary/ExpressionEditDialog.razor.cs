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

    private bool DisableExpressionTextField => _expression is not null;
    
    private string? _error;
    private string _text = string.Empty;
    private Expression? _expression;
    
    private bool DisableSubmit => _expression is null || !_hasChanged;

    private bool _hasChanged;

    private void OnExpressionChanged()
    {
        _hasChanged = true;
    }
    
    private async Task OnTextChangedAsync()
    {
        if (string.IsNullOrWhiteSpace(_text))
            return;

        try
        {
            var translation = await TranslationRepository
                .Find(text: _text, Language.Reference);
            
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
        
            _hasChanged = false;
            
            MudDialog.Close(DialogResult.Ok(_expression));

            _error = "Level: " + _expression.Level;
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
}

