using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public partial class StudyCardView : ComponentBase
{
    [Parameter] public required Action OnClick { get; set; }
    [Parameter] public required StudyCard StudyCard { get; set; }

    private string _audioId = Guid.NewGuid().ToString();
    private Translation? _audio;
    
    protected override void OnParametersSet()
    {
        if (StudyCard.AudioId != null)
        {
            _audio = new Translation(
                0,
                StudyCard.Language,
                StudyCard.Name,
                null,
                StudyCard.AudioId,
                Expression.Create());
        }
        
        base.OnParametersSet();
    }
}

