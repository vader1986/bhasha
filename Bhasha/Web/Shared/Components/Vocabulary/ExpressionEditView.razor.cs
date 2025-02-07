using System.ComponentModel.DataAnnotations;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class ExpressionEditView : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required IResourcesManager ResourcesManager { get; set; }

    [Parameter] public required Expression Value { get; set; }
    [Parameter] public EventCallback<Expression> ValueChanged { get; set; }

    [Range(1, 100)]
    private int _level = 1;
    private ExpressionType? _expressionType;
    private PartOfSpeech? _partOfSpeech;
    private CEFR? _cefr;
    private List<string> _labels = [];
    private List<string> _synonyms = [];
    private string? _resourceId;

    private string? _error;
    
    private async Task OnResourceChanged(IBrowserFile? imageFile)
    {
        try
        {
            if (imageFile is null)
                return;

            await ResourcesManager.UploadImage(imageFile.Name, imageFile.OpenReadStream());

            _resourceId = imageFile.Name;

            await OnValueChanged();
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }
    
    protected override void OnParametersSet()
    {
        _level = Value.Level;
        _expressionType = Value.ExpressionType;
        _partOfSpeech = Value.PartOfSpeech;
        _cefr = Value.Cefr;
        _labels = Value.Labels.ToList();
        _synonyms = Value.Synonyms.ToList();
        _resourceId = Value.ResourceId;
        
        base.OnParametersSet();
    }

    private async Task OnValueChanged()
    {
        await ValueChanged.InvokeAsync(Value with
        {
            Level = _level,
            ExpressionType = _expressionType,
            PartOfSpeech = _partOfSpeech,
            Cefr = _cefr,
            Labels = _labels.ToArray(),
            Synonyms = _synonyms.ToArray(),
            ResourceId = _resourceId
        });
    }
}

