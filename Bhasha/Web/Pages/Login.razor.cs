using Bhasha.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Bhasha.Web.Pages;

public partial class Login : ComponentBase
{
    [Inject] public required SignInManager<AppUser> SignInManager { get; set; }
    
    private readonly LoginModel _model = new()
    {
        Username = string.Empty,
        Password = string.Empty
    };

    private async Task LoginAsync()
    {
        await SignInManager
            .PasswordSignInAsync(
                userName: _model.Username, 
                password: _model.Password, 
                isPersistent: true, 
                lockoutOnFailure: false);
    }
    
    private class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}