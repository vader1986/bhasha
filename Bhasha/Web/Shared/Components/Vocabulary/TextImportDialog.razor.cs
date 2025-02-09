using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TextImportDialog : ComponentBase
{
    [Inject] public required IExpressionRepository ExpressionRepository { get; set; }
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    [Inject] public required ITranslator Translator { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }

    private bool DisableSave => _error is not null || string.IsNullOrWhiteSpace(_text);
    
    private readonly ExpressionEditViewModel _viewModel = new();
    private string? _error;
    private string _text = string.Empty;

    private void OnCancel()
    {
        MudDialog.Cancel();
    }
    
    private async Task OnSaveAsync()
    {
        if (DisableSave)
            return;
        
        try
        {
            var words = _text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            var referenceTranslations = new List<Translation>();
            
            foreach (var word in words)
            {
                var translation = await TranslationRepository
                    .Find(text: word, Language.Reference);

                if (translation is null)
                { 
                    referenceTranslations.Add(new Translation(
                        Id: 0,
                        Language: Language.Reference, 
                        Text: word, 
                        Spoken: null, 
                        AudioId: null, 
                        Expression: _viewModel.ToExpression()));
                }
            }

            foreach (var referenceTranslation in referenceTranslations)
            {
                var expression = await ExpressionRepository
                    .AddOrUpdate(referenceTranslation.Expression);
                
                await TranslationRepository
                    .AddOrUpdate(referenceTranslation with { Expression = expression });

                foreach (var language in Language.Supported.Values.Where(lng => lng != Language.Reference))
                {
                    var (translation, transliteration) = await Translator.Translate(referenceTranslation.Text, language);
                    
                    if (string.IsNullOrWhiteSpace(translation))
                        continue;

                    if (string.IsNullOrWhiteSpace(transliteration))
                        transliteration = null;
                    
                    await TranslationRepository
                        .AddOrUpdate(new Translation(
                            Id: 0,
                            Language: language,
                            Text: translation,
                            Spoken: transliteration,
                            AudioId: null, 
                            Expression: expression));
                }
            }
            
            MudDialog.Cancel();
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
}

