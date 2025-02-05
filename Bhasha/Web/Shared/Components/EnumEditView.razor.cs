using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class EnumEditView<T> : ComponentBase where T : struct, Enum
{
    private static string Label => typeof(T).Name;
    private static string[] Values => Enum.GetNames<T>();

    [Parameter] public required T Value { get; set; }
    [Parameter] public EventCallback<T> ValueChanged { get; set; }

    private string _selectedValue = "";
    
    protected override void OnParametersSet()
    {
        _selectedValue = Value.ToString();

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
    }
}

