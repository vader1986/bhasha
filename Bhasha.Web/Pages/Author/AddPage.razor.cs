using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages.Author
{
	public partial class AddPage : ComponentBase
	{
		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		private AddPageState _state = new AddPageState();

		internal void Submit()
		{
			MudDialog?.Close(DialogResult.Ok(_state));
		}

		internal void Cancel()
        {
			MudDialog?.Cancel();
		}
	}
}

