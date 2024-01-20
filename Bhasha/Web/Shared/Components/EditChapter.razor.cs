using Bhasha.Domain;
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
    
    [Parameter] public required string UserId { get; set; }

    public string? Error { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RequiredLevel { get; set; } = 1;
    public List<(PageType PageType, string Expression)> Pages { get; } = new();

    private bool DisableSave =>
        string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || Pages.Count < 3 || Error != null;

    internal async Task OpenAddTranslation()
    {
        try
        {
            var dialog = await DialogService.ShowAsync<AddTranslation>("Add Translation");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var (dlgLanguage, dlgExpression, dlgTranslation) = ((string Language, string Expression, string Translation))result.Data;

                var expression = await AuthoringService
                    .GetOrCreateExpression(dlgExpression, RequiredLevel);
                
                var translation = Translation
                    .Create(expression, dlgLanguage, dlgTranslation);

                await AuthoringService
                    .AddOrUpdateTranslation(translation);
            }
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    internal async Task OpenAddPage()
    {
        try
        {
            var dialog = await DialogService.ShowAsync<AddPage>("Add Page");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var data = ((PageType, string))result.Data;
                Pages.Add(data);
            }
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    internal async Task Submit()
    {
        try
        {
            var name = await AuthoringService.GetOrCreateExpression(Name, RequiredLevel);
            var description = await AuthoringService.GetOrCreateExpression(Description, RequiredLevel);

            var pages = new List<Expression>();

            foreach (var page in Pages)
            {
                var expression = await AuthoringService
                    .GetOrCreateExpression(page.Expression, RequiredLevel);
                
                pages.Add(expression);
            }

            var chapter = Chapter
                .Create(RequiredLevel, name, description, pages, UserId);
            
            await AuthoringService
                .AddOrUpdateChapter(chapter);
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

