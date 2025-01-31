﻿using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class AddPage : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    public string Expression { get; set; } = string.Empty;

    private bool DisableCreate => string.IsNullOrWhiteSpace(Expression);

    internal void Submit()
    {
        MudDialog?.Close(DialogResult.Ok(Expression));
    }

    internal void Cancel()
    {
        MudDialog?.Cancel();
    }
}

