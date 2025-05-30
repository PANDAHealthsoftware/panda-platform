using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace PANDAView.Services.HttpHandlers;

/// <summary>
/// A delegating handler that appends the JWT bearer token from local storage to outgoing HTTP requests.
/// </summary>
/// <remarks>
/// This handler retrieves an authentication token (JWT) from local storage and adds it to the
/// Authorization header of outgoing HTTP requests. The token is used for authenticating requests to APIs
/// that require a Bearer token.
/// </remarks>
public class AuthTokenHandler : DelegatingHandler
{
    /// <summary>
    /// Represents an instance of the local storage service used for handling
    /// access to the browser's local storage. Provides methods for storing,
    /// retrieving, and managing data in local storage.
    /// </summary>
    private readonly ILocalStorageService _localStorage;

    /// <summary>
    /// Provides a custom HTTP message handler for injecting an authorization token into HTTP requests.
    /// </summary>
    /// <remarks>
    /// The handler retrieves a stored authentication token from local storage and attaches it as a Bearer token
    /// in the Authorization header of outgoing HTTP requests. If no token is available, the request is sent
    /// without modifying the headers.
    /// </remarks>
    public AuthTokenHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// Sends an HTTP request with an optional authorization token retrieved from local storage.
    /// <param name="request">
    /// The HTTP request message to send.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation.
    /// </param>
    /// <return>
    /// A task that represents the asynchronous operation. The task result contains the HTTP response message from the server.
    /// </return>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("auth_token");
        Console.WriteLine($"[AuthHandler] Token: {token}");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"[AuthHandler] Attached token: {token[..Math.Min(30, token.Length)]}...");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}