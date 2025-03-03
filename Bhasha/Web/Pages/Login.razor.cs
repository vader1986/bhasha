using Bhasha.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace Bhasha.Web.Pages;

public partial class Login : ComponentBase
{
    private const string LoginFailedMessage = "Invalid email or password!";
    
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required SignInManager<AppUser> SignInManager { get; set; }
    [Inject] public required UserManager<AppUser> UserManager { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    
    private readonly LoginModel _model = new()
    {
        Email = string.Empty,
        Password = string.Empty
    };

    private async Task LoginAsync()
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(_model.Email);
        
            if (user is null)
            {
                Snackbar.Add(LoginFailedMessage + " where is my user? :(( ", Severity.Error);
                return;
            }
        
            var result = await SignInManager.CheckPasswordSignInAsync(user, _model.Password, false);

            if (result.Succeeded)
            {
                Snackbar.Add("Successful login!", Severity.Success);

                await SignInManager
                    .PasswordSignInAsync(
                        userName: _model.Email, 
                        password: _model.Password, 
                        isPersistent: true, 
                        lockoutOnFailure: false);
                
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(LoginFailedMessage, Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message + ": " + e.StackTrace, Severity.Error);
        }
    }
    
    private class LoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}