using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bhasha.Web.Pages
{
	public class UserPage : ComponentBase
	{
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected string? UserId { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = state.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);

            UserId = userId.Value;
        }
    }
}

