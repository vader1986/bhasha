using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class ExpressionEditDialog : ComponentBase
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required IExpressionRepository ExpressionRepository { get; set; }
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }

    private bool DisableExpressionTextField => _expression is not null;
    
    private string? _error;
    private string _text = string.Empty;
    private Expression? _expression;

    private List<TranslationEditViewModel> _translationEditViewModels = [];
    
    private bool DisableSubmit => _expression is null || (!_hasChanged && !_translationsChanged);
    
    private bool _hasChanged;
    private bool _translationsChanged;

    private void OnExpressionChanged()
    {
        _hasChanged = true;
        
        StateHasChanged();
    }

    private void OnTranslationChanged()
    {
        _translationsChanged = _translationEditViewModels.Any(x => x.Status is not TranslationViewModelStatus.Initial);
        
        StateHasChanged();
    }
    
    private async Task OnTextChangedAsync()
    {
        if (string.IsNullOrWhiteSpace(_text))
            return;

        try
        {
            var translation = await TranslationRepository
                .Find(text: _text, Language.Reference);

            if (translation is not null)
            {
                var translations = await TranslationRepository
                    .Find(translation.Expression.Id);
                
                _translationEditViewModels = translations
                    .Select(TranslationEditViewModel.Create).ToList();
            }
            else
            {
                var viewModel = TranslationEditViewModel
                    .Create(Language.Reference);
                
                viewModel.Status = TranslationViewModelStatus.Created;
                viewModel.Text = _text;
                
                _translationEditViewModels
                    .Add(viewModel);

                _translationsChanged = true;
            }
            
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
        
        if (!_hasChanged && !_translationsChanged)
            return;

        try
        {
            if (_hasChanged)
            {
                await ExpressionRepository.AddOrUpdate(_expression);

                _hasChanged = false;
            }

            if (_translationsChanged)
            {
                foreach (var translationEditViewModel in _translationEditViewModels)
                {
                    var translation = translationEditViewModel
                        .ToTranslation(_expression);

                    switch (translationEditViewModel.Status)
                    {
                        case TranslationViewModelStatus.Changed:
                            await TranslationRepository.AddOrUpdate(translation);
                            break;
                        case TranslationViewModelStatus.Deleted:
                            await TranslationRepository.Delete(translation.Id);
                            break;
                        case TranslationViewModelStatus.Created:
                            await TranslationRepository.AddOrUpdate(translation);
                            break;
                        case TranslationViewModelStatus.Initial:
                            break;
                    }
                }
                
                _translationsChanged = false;   
            }
            
            MudDialog.Close(DialogResult.Ok(_expression));
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
}

