using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaxiNT.Client.Services;
using TaxiNT.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// UI: Register Client Services
builder.Services.AddScoped<ICheckerService, CheckerService>();
builder.Services.AddScoped<ICheckerDetailService, CheckerDetailService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();

builder.Services.AddScoped(
    sp => new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

await builder.Build().RunAsync();
