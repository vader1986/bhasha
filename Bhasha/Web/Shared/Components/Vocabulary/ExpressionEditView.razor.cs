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

    private ExpressionEditViewModel _viewModel = new();

    private string? _error;
    
    private async Task OnResourceChanged(IBrowserFile? imageFile)
    {
        try
        {
            if (imageFile is null)
                return;

            await ResourcesManager.UploadImage(imageFile.Name, imageFile.OpenReadStream());

            _viewModel.ResourceId = imageFile.Name;

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
        _viewModel = ExpressionEditViewModel.Create(Value);
        
        base.OnParametersSet();
    }

    private async Task OnValueChanged()
    {
        await ValueChanged.InvokeAsync(_viewModel.ToExpression());
    }
}

