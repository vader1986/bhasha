using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class MultipleChoicePage : ComponentBase, IDisplayPage
{
    private const int MaxNumberOfChoices = 4;

    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required ITranslationProvider Translations { get; set; }
    [Parameter] public required ChapterPageViewModel ViewModel { get; set; }
    [Parameter] public required string? Value { get; set; }
    [Parameter] public required EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public required EventCallback<Exception> OnError { get; set; }

    private string? _selectedChoice;
    private Translation[] _choices = [];
    private string? _resourceId;
    private DisplayedPage? _page;

    private async Task CreateChoicesAsync()
    {
        var translations = new List<Translation>();

        try
        {
            foreach (var page in ViewModel.Chapter.Pages)
            {
                var translation = await Translations
                    .Find(page.Word.Expression.Id, ViewModel.ProfileKey.Target);

                if (translation != null)
                    translations.Add(translation);
            }

            var count = translations.Count;
            var choices = translations
                .Where(TranslationIsWrong)
                .OrderBy(Randomness)
                .Take(NumberOfWrongChoices(count))
                .Append(CorrectTranslation())
                .Select(TranslationWithHiddenExpression)
                .ToArray();

            Random.Shared.Shuffle(choices);
            
            _choices = choices;
            _resourceId = ViewModel.Page.Word.Expression.ResourceId;
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
        
        Translation TranslationWithHiddenExpression(Translation translation)
            => translation with { Expression = Expression.Create(translation.Expression.Level) };
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

