using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class EnumEditView<T> : ComponentBase where T : struct, Enum
{
    private const string NoSelection = "No selection";
    
    [Parameter] public required T? Value { get; set; }
    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    private string _selectedValue = NoSelection;
    
    private static string[] EnumValues => Enum.GetNames<T>();
    private static string Label => typeof(T).Name;
    
    protected override void OnParametersSet()
    {
        _selectedValue = Value?.ToString() ?? NoSelection;

        base.OnParametersSet();
    }

    private async Task OnValueChanged(string value)
    {
        _selectedValue = value;

        var selectedEnumName = Enum
            .GetNames(typeof(T))
            .FirstOrDefault();

        if (Enum.TryParse<T>(selectedEnumName, out var actualValue))
        {
            await ValueChanged.InvokeAsync(actualValue);
        }
        else
        {
            await ValueChanged.InvokeAsync(null);
        }
    }
}

