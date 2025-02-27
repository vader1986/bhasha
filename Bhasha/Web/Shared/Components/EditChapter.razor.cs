﻿using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Chapter = Bhasha.Domain.Chapter;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Web.Shared.Components;

public partial class EditChapter : ComponentBase
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required IAuthoringService AuthoringService { get; set; }
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }
    [Inject] public required IChapterRepository ChapterRepository { get; set; }
  
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }

    [Parameter] public required string UserId { get; set; }
    [Parameter] public string TargetLanguage { get; set; } = Language.Bengali;
    [Parameter] public Chapter? Chapter { get; set; }
    
    private string Name { get; set; } = string.Empty;
    private string Description { get; set; } = string.Empty;
    private int RequiredLevel { get; set; } = 1;
    private List<string> Pages { get; } = new();

    private string? Error { get; set; }

    private bool DisableSave =>
        string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || Pages.Count < 3 || Error != null;
    
    private bool DisableDelete => Chapter is null;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (Chapter is null)
            return;
        
        var name = await TranslationProvider.Find(Chapter.Name.Id, Language.Reference);
        var description = await TranslationProvider.Find(Chapter.Description.Id, Language.Reference);

        Name = name?.Text ?? "";
        Description = description?.Text ?? "";
        RequiredLevel = Chapter.RequiredLevel;
        Pages.Clear();

        foreach (var page in Chapter.Pages)
        {
            var translation = await TranslationProvider.Find(page.Id, Language.Reference);
            if (translation is null)
            {
                Error = $"Could not find translation for expression {page.Id}";
            }
            else
            {
                Pages.Add(translation.Text);
            }
        }
    }
    
    private async Task OnAddPage()
    {
        try
        {
            var dialog = await DialogService.ShowAsync<AddPage>("Add Page");
            var result = await dialog.Result;

            if (result is { Canceled: false, Data: not null })
            {
                Pages.Add((string)result.Data);
            }
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }

    private async Task<bool> TryRequestTranslation(string reference)
    {
        var title = "Translation";
        var args = new DialogParameters
        {
            { "Language", TargetLanguage },
            { "ReferenceTranslation", reference }
        };
        
        var dialog = await DialogService.ShowAsync<TranslationDialog>(title, args);
        var result = await dialog.Result;
        
        return result is not null && !result.Canceled;
    }

    private async Task OnDelete()
    {
        if (Chapter is null)
            return;

        try
        {
            await ChapterRepository.Delete(Chapter.Id, CancellationToken.None);
            MudDialog.Close(DialogResult.Cancel());
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
    
    private async Task OnSubmit()
    {
        try
        {
            var name = await AuthoringService.GetOrCreateExpression(Name, RequiredLevel);
            var description = await AuthoringService.GetOrCreateExpression(Description, RequiredLevel);

            var pages = new List<Expression>();

            foreach (var page in Pages)
            {
                var expression = await AuthoringService
                    .GetOrCreateExpression(page, RequiredLevel);

                var translation = await TranslationProvider
                    .Find(expression.Id, TargetLanguage);

                if (translation is null)
                {
                    if (await TryRequestTranslation(page) is false)
                    {
                        Error = $"Please add a translation for {page}";
                        return;
                    }
                }
                
                pages.Add(expression);
            }

            var chapter = Chapter is null 
                    ? new Chapter(default, RequiredLevel, name, description, pages.ToArray(), default, UserId) 
                    : new Chapter(Chapter.Id, RequiredLevel, name, description, pages.ToArray(), Chapter.ResourceId, UserId);

            Chapter = chapter;
            
            MudDialog.Close(DialogResult.Ok(chapter));
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

