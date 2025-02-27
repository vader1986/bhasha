using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Student;

public partial class SubmitButton : ComponentBase
{
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required IStudyingService StudyingService { get; set; }

    [Parameter] public required ProfileKey ProfileKey { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    
    [Parameter] public required int ExpressionId { get; set; }
    [Parameter] public Translation? Selection { get; set; }
    
    [Parameter] public EventCallback<Exception>? OnError { get; set; }

    private bool Disabled => Selection is null;
    
    private async Task SubmitAsync()
    {
        if (Selection is null)
            return;
        
        try
        {
            var userInput = new ValidationInput(
                Key: ProfileKey,
                ExpressionId: ExpressionId, 
                Translation: Selection);

            var validation = await StudyingService
                .Submit(userInput);

            switch (validation.Result)
            {
                case ValidationResult.Correct:
                    Snackbar.Add("Correct!", Severity.Success);
                    break;
                
                case ValidationResult.PartiallyCorrect:
                    Snackbar.Add("Almost correct!", Severity.Success);
                    break;
                
                case ValidationResult.Wrong:
                    Snackbar.Add("Wrong!", Severity.Error);
                    break;
            }

            await OnClick.InvokeAsync();
        }
        catch (Exception error)
        {
            if (OnError is not null)
            {
                await OnError.Value.InvokeAsync(error);
            }
        }
    }
}

