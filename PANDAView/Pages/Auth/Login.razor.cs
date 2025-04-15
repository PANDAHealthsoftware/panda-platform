using Microsoft.AspNetCore.Components;
using PANDAView.Models;
using PANDAView.Services.Authentication;

namespace PANDAView.Pages.Auth;

public class LoginBase : ComponentBase
{
    protected LoginModel Credentials { get; set; } = new();
    protected string? ErrorMessage;
    protected bool IsLoading { get; set; }

    protected async Task HandleLogin()
    {
        IsLoading = true;
        ErrorMessage = null;

        Credentials.Username = Credentials.Username.Trim();

        var result = await AuthService.LoginAsync(Credentials);

        IsLoading = false;

        if (!result)
        {
            ErrorMessage = "Invalid credentials.";
        }
        else
        {
            NavigationManager.NavigateTo("/home");
        }
    }

    [Inject] protected IAuthService AuthService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
}