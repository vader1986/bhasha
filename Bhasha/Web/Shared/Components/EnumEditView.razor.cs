using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class EnumEditView<T> : ComponentBase where T : struct, Enum
{
    private const string None = "None";
    private static string Label => typeof(T).Name;
    private static string[] Values => Enum.GetNames<T>().Append(None).ToArray();

    [Parameter] public required T? Value { get; set; }
    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    private string _selectedValue = None;
    
    protected override void OnParametersSet()
    {
        _selectedValue = Value?.ToString() ?? None;

        base.OnParametersSet();
    }

    private async Task OnValueChanged()
    {
        if (Enum.TryParse<T>(_selectedValue, out var actualValue))
        {
            await ValueChanged.InvokeAsync(actualValue);
        }
        else
        {
            await ValueChanged.InvokeAsync(null);
        }
    }
}

