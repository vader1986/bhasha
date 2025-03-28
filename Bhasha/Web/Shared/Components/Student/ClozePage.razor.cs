﻿using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ClozePage : ComponentBase, IDisplayPage
{
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }
    
    [Parameter] public required ChapterPageViewModel ViewModel { get; set; }
    [Parameter] public required string? Value { get; set; }
    [Parameter] public required EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public required EventCallback<Exception> OnError { get; set; }
    
    private DisplayedPage? _page;

    private int[] _gaps = [];
    private SortedDictionary<int, string> _choices = [];
    private SortedDictionary<int, string> _tokens = [];
    private string _word = string.Empty;
    private string? _spoken;
    private List<int> _openTokens = [];
    
    private bool IsOpenChoice(int index) 
        => !_choices.ContainsKey(index) && _gaps.Contains(index);
    
    private async Task CreateClozeAsync()
    {
        try
        {
            var translation = await TranslationProvider
                .Find(
                    ViewModel.Page.Word.Expression.Id, 
                    ViewModel.ProfileKey.Target);
            
            if (translation is null)
                return;

            _spoken = translation.Spoken;
            
            var words = translation.Text.Split(" ");

            var (tokens, gaps) = words.Length >= 3 
                ? CreateClozeFrom(words) 
                : CreateClozeFrom(translation.Text);

            _gaps = gaps;

            _tokens = new SortedDictionary<int, string>(tokens
                .Select((token, index) => (Index: index, Token: token))
                .ToDictionary(x => x.Index, x => x.Token));
            
            _choices = new SortedDictionary<int, string>(tokens
                .Select((token, index) => (Index: index, Token: token))
                .Where(x => !_gaps.Contains(x.Index))
                .ToDictionary(x => x.Index, x => x.Token));
            
            _openTokens = _gaps
                .Where(IsOpenChoice)
                .ToList();
        }
        catch (Exception e)
        {
            await OnError.InvokeAsync(e);
        }
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _word = ViewModel.Page.Word.Text;
        
        if (_page != ViewModel.Page)
        {
            _page = ViewModel.Page;
            
            await CreateClozeAsync();
        }
    }

    private async Task UpdateValueAsync()
    {
        if (_tokens.Count == 0)
            return;

        if (_tokens.Count != _choices.Count)
        {
            if (_openTokens.Count == 1)
            {
                await ValueChanged.InvokeAsync(null);
            }
        }
        else
        {
            var userInput = _choices.OrderBy(x => x.Key).Select(x => x.Value);

            var value = string.Join(string.Empty, userInput);
        
            await ValueChanged.InvokeAsync(value);
        }
        
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ChooseAsync(MudChip<int> chip)
    {
        var index = _tokens.Keys
            .First(IsOpenChoice);
        
        _choices.Add(index, _tokens[chip.Value]);
        _openTokens.Remove(chip.Value);

        await UpdateValueAsync();
    }
    
    private async Task UnChooseAsync(MudChip<int> chip)
    {
        var index = _tokens
            .First(x => x.Value == _choices[chip.Value])
            .Key;
        
        _choices.Remove(chip.Value);
        _openTokens.Add(index);

        await UpdateValueAsync();
    }
    
    private static (string[] Tokens, int[] Gaps) CreateClozeFrom(string word)
    {
        var tokens = word
            .Select(character => character.ToString())
            .ToArray();
        
        var gaps = tokens
            .Select((_, index) => index)
            .Where(index => index % 2 == 0) // every 3rd character is a "gap"
            .ToArray();

        return(Tokens: tokens, Gaps: gaps);
    }
    
    private static (string[] Tokens, int[] Gaps) CreateClozeFrom(string[] words)
    {
        var tokens = words
            .Take(words.Length - 1)
            .SelectMany(word => new[] { word, " " })
            .Append(words.Last())
            .ToArray();

        var gaps = tokens
            .Select((word, index) => (Word: word, Index: index))
            .Where(x => x.Word != " ")   // don't consider spaces
            .Where((_, i) => i % 2 == 0) // every 3rd word is a "gap"
            .Select(x => x.Index)
            .ToArray();
        
        Random.Shared.Shuffle(gaps);
        
        return (Tokens: tokens, Gaps: gaps);
    }
}

