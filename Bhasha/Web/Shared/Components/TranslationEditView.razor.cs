using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class TranslationEditView : ComponentBase
{
    [Parameter] public bool DisplayLanguage { get; set; } = false;
    [Parameter] public required Translation Value { get; set; }
    [Parameter] public EventCallback<Translation> ValueChanged { get; set; }

    private string _text = string.Empty;
    private string? _spoken;

    protected override void OnParametersSet()
    {
        _text = Value.Text;
        _spoken = Value.Spoken;
        
        base.OnParametersSet();
    }

    private async Task OnFocusLost()
    {
        if (string.IsNullOrWhiteSpace(_spoken))
        {
            _spoken = null;
        }
        
        await ValueChanged.InvokeAsync(Value with
        {
            Text = _text,
            Spoken = _spoken
        });
    }
}

