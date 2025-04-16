using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace PANDAView.Pages.Shared;

public class HomeBase : ComponentBase
{
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    protected bool IsAuthenticated;
    protected bool IsAdmin;
    protected string? UserName;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
        IsAdmin = user.IsInRole("Admin");
        UserName = user.Identity?.Name ?? user.FindFirst(c => c.Type == "sub")?.Value;
    }
}