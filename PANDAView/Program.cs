using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PANDAView;
using PANDAView.Services.Appointment;
using PANDAView.Services.Clinician;
using PANDAView.Services.Patient;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IClinicianService, ClinicianService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddRadzenComponents();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44372/") });

await builder.Build().RunAsync();