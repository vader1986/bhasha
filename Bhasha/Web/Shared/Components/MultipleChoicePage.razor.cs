using Bhasha.Domain;
using Bhasha.Domain.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class MultipleChoicePage : ComponentBase
{
    [Parameter] public DisplayedPage<MultipleChoice> Data { get; set; }
    [Parameter] public Action<Translation> Submit { get; set; }

    private bool DisableSubmit => _selectedChoice == null;

    private MultipleChoice? _arguments;
    private MudChip? _selectedChoice;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _selectedChoice = null;
        _arguments = Data?.Arguments;
    }

    internal void OnSubmit()
    {
        if (_selectedChoice == null)
            return;

        Submit((Translation)_selectedChoice.Value);
    }
}

