using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChapterPageBar : ComponentBase
{
    [Parameter] public required ChapterPageBarViewModel ViewModel { get; set; }
    [Parameter] public required EventCallback OnSubmit { get; set; }
    [Parameter] public required EventCallback<Exception> OnError { get; set; }
}

