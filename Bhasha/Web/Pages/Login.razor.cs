using Bhasha.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace Bhasha.Web.Pages;

public partial class Login : ComponentBase
{
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required SignInManager<AppUser> SignInManager { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    
    private readonly LoginModel _model = new()
    {
        Username = string.Empty,
        Password = string.Empty
    };

    private async Task LoginAsync()
    {
        var result = await SignInManager.CheckPasswordSignInAsync(new AppUser
        {
            UserName = _model.Username
        }, _model.Password, false);

        if (result.Succeeded)
        {
            await SignInManager
                .PasswordSignInAsync(
                    userName: _model.Username, 
                    password: _model.Password, 
                    isPersistent: true, 
                    lockoutOnFailure: false);
            
            Snackbar.Add("Successful login!", Severity.Success);

            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add("Invalid username or password!", Severity.Error);
        }
    }
    
    private class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}