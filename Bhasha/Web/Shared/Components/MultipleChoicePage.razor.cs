using Bhasha.Domain;
using Bhasha.Domain.Pages;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class MultipleChoicePage : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Parameter] public required MultipleChoice MultipleChoice { get; set; }
    [Parameter] public required Translation? Value { get; set; }
    [Parameter] public required EventCallback<Translation?> ValueChanged { get; set; }

    private Translation? _selectedChoice;
    
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _selectedChoice = Value;
    }
    
    private async Task OnValueChanged()
    {
        await ValueChanged.InvokeAsync(_selectedChoice);
    }
}

