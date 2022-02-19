using System.Security.Claims;
using Bhasha.Web.Domain;
using MudBlazor;

namespace Bhasha.Web.Pages
{
    public partial class AddChapter
    {
        private AddChapterState _state = new AddChapterState();

        protected override async Task OnInitializedAsync()
        {
            _state.ChapterRepository = ChapterRepository;
            _state.ExpressionFactory = ExpressionFactory;
            _state.ExpressionManager = ExpressionManager;

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = state.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);

            _state.UserId = userId.Value;
        }

        private void OnSelectedNative(MudChip chip)
        {
            _state.NativeLanguage = (chip?.Value as Language)?.ToString();
        }

        private void OnSelectedTarget(MudChip chip)
        {
            _state.TargetLanguage = (chip?.Value as Language)?.ToString();
        }

        private async Task OpenCreatePage()
        {
            var result = await DialogService.Show<AddPage>("Create Page").Result;

            if (!result.Cancelled)
            {
                await _state.SubmitPageState((AddPageState)result.Data);
            }
        }
    }
}