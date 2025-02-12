using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bhasha.Web.Pages
{
	public class UserPage : ComponentBase
	{
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject] public required NavigationManager Navigation { get; set; }

        protected string UserId { get; private set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var userId = state.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);

                UserId = userId.Value;
            }
            catch
            {
                Navigation.NavigateTo("/Identity/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(UserId))
            {
                Navigation.NavigateTo("/Identity/Account/Login");
            }
        }
    }
}

