using Bhasha.Domain;
using Bhasha.Shared.Domain;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class AddPage : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    public PageType PageType { get; set; } = PageType.MultipleChoice;
    public string Expression { get; set; } = string.Empty;

    private bool DisableCreate => string.IsNullOrWhiteSpace(Expression);

    internal void Submit()
    {
        MudDialog?.Close(DialogResult.Ok<(PageType PageType, string Expression)>(new (PageType, Expression)));
    }

    internal void Cancel()
    {
        MudDialog?.Cancel();
    }
}

