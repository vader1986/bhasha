using Bhasha.Domain;
using Bhasha.Grains;
using Bhasha.Web.Pages.Author;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class EditChapter : ComponentBase
{
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public IClusterClient ClusterClient { get; set; }
    [Parameter] public string UserId { get; set; }

    public string? Error { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RequiredLevel { get; set; } = 1;
    public List<(PageType PageType, string Expression)> Pages { get; } = new List<(PageType, string)>();

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
                var grain = ClusterClient.GetGrain<IAuthorGrain>(UserId);
                var expressionId = await grain.GetOrAddExpressionId(expression);
                var translation = Translation.Create(expressionId, language, text);

                await grain.AddOrUpdateTranslation(translation);
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
            var grain = ClusterClient.GetGrain<IAuthorGrain>(UserId);

            
            var nameId = await grain.GetOrAddExpressionId(Name);
            var descriptionId = await grain.GetOrAddExpressionId(Description);

            var pages = new List<Domain.Page>();

            foreach (var page in Pages)
            {
                var expressionId = await grain.GetOrAddExpressionId(page.Expression);
                pages.Add(new Domain.Page(page.PageType, expressionId));
            }

            var chapter = Chapter.Create(RequiredLevel, nameId, descriptionId, pages, UserId);
            await grain.AddOrUpdateChapter(chapter);
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

