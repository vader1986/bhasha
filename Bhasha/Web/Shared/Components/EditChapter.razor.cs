using Bhasha.Domain;
using Bhasha.Services;
using Bhasha.Shared.Domain;
using Bhasha.Web.Pages.Author;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Page = Bhasha.Shared.Domain.Page;

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
            var dialog = DialogService.Show<AddTranslation>("Add Translation");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var (language, expression, text) = ((string Language, string Expression, string Translation))result.Data;
                var expressionId = await AuthoringService.GetExpressionId(expression, RequiredLevel);
                var translation = Translation.Create(expressionId, language, text);

                await AuthoringService.AddOrUpdateTranslation(translation);
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
            var dialog = DialogService.Show<AddPage>("Add Page");
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
            var nameId = await AuthoringService.GetExpressionId(Name, RequiredLevel);
            var descriptionId = await AuthoringService.GetExpressionId(Description, RequiredLevel);

            var pages = new List<Page>();

            foreach (var page in Pages)
            {
                var expressionId = await AuthoringService.GetExpressionId(page.Expression, RequiredLevel);
                pages.Add(new Page(page.PageType, expressionId));
            }

            var chapter = Chapter.Create(RequiredLevel, nameId, descriptionId, pages, UserId);
            await AuthoringService.AddOrUpdateChapter(chapter);
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

