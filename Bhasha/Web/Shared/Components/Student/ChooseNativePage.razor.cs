using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChooseNativePage : ComponentBase, IDisplayPage
{
    private sealed record Choice(string Native, string Target, string? ResourceId);

    private const int MaxNumberOfChoices = 4;

    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required ITranslationProvider Translations { get; set; }
    [Parameter] public required ChapterPageViewModel ViewModel { get; set; }
    [Parameter] public required string? Value { get; set; }
    [Parameter] public required EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public required EventCallback<Exception> OnError { get; set; }

    private string? _selectedChoice;
    private Choice[] _choices = [];
    private string _word = string.Empty;
    private DisplayedPage? _page;
    
    private bool DisplayImages => _choices
        .All(choice => choice.ResourceId is not null);

    private async Task CreateChoicesAsync()
    {
        var translations = new List<Translation>();

        var targetWord = await Translations
            .Find(ViewModel.Page.Word.Expression.Id, ViewModel.ProfileKey.Target);

        _word = targetWord?.Text ?? string.Empty;

        try
        {
            foreach (var page in ViewModel.Chapter.Pages)
            {
                var translation = await Translations
                    .Find(page.Word.Expression.Id, ViewModel.ProfileKey.Native);

                if (translation != null)
                    translations.Add(translation);
            }

            var count = translations.Count;
            var choices = translations
                .Where(TranslationIsWrong)
                .OrderBy(Randomness)
                .Take(NumberOfWrongChoices(count))
                .Append(CorrectTranslation())
                .Select(translation => new Choice(
                    Native: translation.Text,
                    Target: targetWord?.Text ?? string.Empty,
                    ResourceId: translation.Expression.ResourceId))
                .ToArray();

            Random.Shared.Shuffle(choices);
            
            _choices = choices;
        }
        catch (Exception e)
        {
            await OnError.InvokeAsync(e);
        }
        
        return;
        
        bool TranslationIsWrong(Translation translation)
            => translation.Expression.Id != ViewModel.Page.Word.Expression.Id;
        
        int Randomness(Translation _)
            => Random.Shared.Next();

        int NumberOfWrongChoices(int totalCount)
            => Math.Min(MaxNumberOfChoices - 1, totalCount);
        
        Translation CorrectTranslation()
            => translations.First(x => x.Expression.Id == ViewModel.Page.Word.Expression.Id);
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        _selectedChoice = Value;
        
        if (_page != ViewModel.Page)
        {
            _page = ViewModel.Page;
            
            await CreateChoicesAsync();
        }
    }
    
    private async Task OnValueChanged()
    {
        await ValueChanged.InvokeAsync(_selectedChoice);
    }
}

