using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class EnumEditView<T> : ComponentBase where T : struct, Enum
{
    private const string NoSelection = "None";
    
    private static string Label => typeof(T).Name;
    private static string[] Values => Enum.GetNames(typeof(T)).Append("None").ToArray();

    [Parameter] public required T? Value { get; set; }
    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    private string _selectedValue = NoSelection;
    
    protected override void OnParametersSet()
    {
        _selectedValue = Value?.ToString() ?? NoSelection;

        base.OnParametersSet();
    }

    private async Task OnValueChanged(string value)
    {
        if (_selectedValue == value)
            return;
        
        _selectedValue = value;
        
        if (Enum.TryParse<T>(value, out var actualValue))
        {
            await ValueChanged.InvokeAsync(actualValue);
        }
        else
        {
            await ValueChanged.InvokeAsync(null);
        }
    }
}

