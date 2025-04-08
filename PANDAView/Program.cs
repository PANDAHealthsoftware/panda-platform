using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PANDAView;
using PANDAview.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddRadzenComponents();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44372/") });

await builder.Build().RunAsync();