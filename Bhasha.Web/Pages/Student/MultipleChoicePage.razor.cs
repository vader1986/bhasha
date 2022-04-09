using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages.Student
{
	public partial class MultipleChoicePage
	{
        [Inject]
        public ISubmissionManager SubmissionManager { get; set; } = default!;

        [Parameter]
        public DisplayedPage? Data { get; set; }

        [Parameter]
        public Func<Translation, Task>? Submit { get; set; }

        private MultipleChoice? _arguments;
        private MudChip? _selectedChoice;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _selectedChoice = null;
            _arguments = (Data as DisplayedPage<MultipleChoice>)?.Arguments;
        }

        internal async Task OnSubmit()
        {
            if (Submit == null)
                return;

            if (_selectedChoice == null)
                return;

            var translation = (Translation)_selectedChoice.Value;

            await Submit(translation);
        }
    }
}

