﻿using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class ExpressionEditDialog : ComponentBase
{
    [Inject] public required IExpressionRepository ExpressionRepository { get; set; }
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }

    public string? Text { get; set; }
    private string? _error;
    private Expression? _expression;
    
    private bool DisableSubmit => _expression is null;

    private async Task OnBlurAsync()
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;
        
        try
        {
            var translation = await TranslationRepository
                .Find(text: Text, Language.Reference);

            _expression = translation is null ? Expression.Create() : translation.Expression;

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
            _error = e.Message;
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

