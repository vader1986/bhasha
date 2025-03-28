﻿using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChooseNativePage : ComponentBase, IDisplayPage
{
    private sealed record Choice(string Native, string Target, bool IsCorrect);

    private const int MaxNumberOfChoices = 4;

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
            IsCorrect: native.Expression.Id == ViewModel.Page.Word.Expression.Id);
    }
    
    private async Task CreateChoicesAsync()
    {
        var availableChoices = new List<Choice>();

        var targetWord = await Translations
            .Find(expressionId: ViewModel.Page.Word.Expression.Id, language: ViewModel.ProfileKey.Target);

        _word = targetWord?.Text ?? string.Empty;

        try
        {
            foreach (var page in ViewModel.Chapter.Pages)
            {
                availableChoices.Add(item: await CreateChoiceAsync(native: page.Word));
            }

            var numberOfWrongChoices = Math
                .Min(val1: MaxNumberOfChoices - 1, val2: availableChoices.Count - 1);

            var choices = availableChoices
                .Where(choice => !choice.IsCorrect)
                .OrderBy(Random)
                .Take(numberOfWrongChoices)
                .Append(availableChoices
                    .First(choice => choice.IsCorrect))
                .ToArray();

            System.Random.Shared.Shuffle(choices);
            
            _choices = choices;
        }
        catch (Exception e)
        {
            await OnError.InvokeAsync(arg: e);
        }
        
        return;
        
        int Random(Choice _)
            => System.Random.Shared.Next();
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

