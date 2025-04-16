using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PANDAView;
using PANDAView.Services.Appointment;
using PANDAView.Services.Authentication;
using PANDAView.Services.Clinician;
using PANDAView.Services.HttpHandlers;
using PANDAView.Services.Patient;
using ProtectedLocalStore;
using Radzen;
using PANDAView.Constants;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root component setup
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var baseApiUrl = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:44372/");

// ---------------------------------------------
// DI: Application & Auth Services
// ---------------------------------------------

builder.Services.AddScoped<AuthTokenHandler>();

// AppointmentService with auth handler
builder.Services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
{
    client.BaseAddress = new Uri(ApiConfig.BaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

// ClinicianService and PatientService (add handlers if needed)
builder.Services.AddHttpClient<IClinicianService, ClinicianService>(client =>
{
    client.BaseAddress = new Uri(ApiConfig.BaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IPatientService, PatientService>(client =>
{
    client.BaseAddress = new Uri(ApiConfig.BaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

// AuthService with base address (no token needed for login)
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(ApiConfig.BaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

// ---------------------------------------------
// Auth & State Providers
// ---------------------------------------------
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// ---------------------------------------------
// UI Framework Services
// ---------------------------------------------
builder.Services.AddRadzenComponents();

// ---------------------------------------------
// Run the app
// ---------------------------------------------
await builder.Build().RunAsync();
