using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterPageBar : ComponentBase
{
    [Parameter] public required int ExpressionId { get; set; }
    [Parameter] public required Translation? Selection { get; set; }
    [Parameter] public required ProfileKey ProfileKey { get; set; }
    [Parameter] public required EventCallback OnSubmit { get; set; }
}

