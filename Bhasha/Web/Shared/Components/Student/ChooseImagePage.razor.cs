using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChooseImagePage : ComponentBase, IDisplayPage
{
    private sealed record Choice(string Native, string Target, string ResourceId, bool IsCorrect);

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

    private async Task<Choice> CreateChoiceAsync(Translation native)
    {
        var target = await Translations
            .Find(native.Expression.Id, ViewModel.ProfileKey.Target);
        
        return new Choice(
            Native: native.Text,
            Target: target?.Text ?? string.Empty,
            ResourceId: native.Expression.ResourceId ?? string.Empty,
            IsCorrect: native.Expression.Id == ViewModel.Page.Word.Expression.Id);
    }

    private async Task CreateChoicesAsync()
    {
        var availableChoices = new List<Choice>();

        var targetWord = await Translations
            .Find(ViewModel.Page.Word.Expression.Id, ViewModel.ProfileKey.Target);

        _word = targetWord?.Text ?? string.Empty;

        try
        {
            var pagesWithImages = ViewModel.Chapter.Pages
                .Where(x => x.Word.Expression.ResourceId is not null);
            
            foreach (var page in pagesWithImages)
            {
                availableChoices.Add(await CreateChoiceAsync(page.Word));
            }

            var numberOfWrongChoices = Math
                .Min(MaxNumberOfChoices - 1, availableChoices.Count - 1);

            var choices = availableChoices
                .Where(choice => !choice.IsCorrect)
                .OrderBy(Randomness)
                .Take(numberOfWrongChoices)
                .Append(availableChoices
                    .First(choice => choice.IsCorrect))
                .ToArray();

            Random.Shared.Shuffle(choices);
            
            _choices = choices;
        }
        catch (Exception e)
        {
            await OnError.InvokeAsync(e);
        }
        
        return;
        
        int Randomness(Choice _)
            => Random.Shared.Next();
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

