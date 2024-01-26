using System.ComponentModel.DataAnnotations;
using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ExpressionEditView : ComponentBase
{
    [Parameter] public required Expression Value { get; set; }
    [Parameter] public EventCallback<Expression> ValueChanged { get; set; }

    [Range(1, 100)]
    private int _level = 1;
    private ExpressionType? _expressionType;
    private PartOfSpeech? _partOfSpeech;
    private CEFR? _cefr;
    private List<string> _labels = new();
    private List<string> _synonyms = new();

    protected override void OnParametersSet()
    {
        _level = Value.Level;
        _expressionType = Value.ExpressionType;
        _partOfSpeech = Value.PartOfSpeech;
        _cefr = Value.Cefr;
        _labels = Value.Labels.ToList();
        _synonyms = Value.Synonyms.ToList();
        
        base.OnParametersSet();
    }

    private async Task OnFocusLost()
    {
        await ValueChanged.InvokeAsync(Value with
        {
            Level = _level,
            ExpressionType = _expressionType,
            PartOfSpeech = _partOfSpeech,
            Cefr = _cefr,
            Labels = _labels.ToArray(),
            Synonyms = _synonyms.ToArray()
        });
    }
}

