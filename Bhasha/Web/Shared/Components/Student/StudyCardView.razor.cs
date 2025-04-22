using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class StudyCardView : ComponentBase
{
    [Parameter] public required Action OnClick { get; set; }
    [Parameter] public required StudyCard StudyCard { get; set; }
}

