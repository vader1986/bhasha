using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class AddTranslation : ComponentBase
{
    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    public string Language { get; set; } = Bhasha.Shared.Domain.Language.Bengali;
    public string Expression { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;

    private bool DisableCreate =>
        string.IsNullOrWhiteSpace(Expression) || string.IsNullOrWhiteSpace(Translation);

    internal void Submit()
    {
        MudDialog.Close(DialogResult.Ok<(string Language, string Expression, string Translation)>(new (Language, Expression, Translation)));
    }

    internal void Cancel()
    {
        MudDialog.Cancel();
    }
}

