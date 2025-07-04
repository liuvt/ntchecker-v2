﻿using TaxiNT.Services;
using TaxiNT.Components;
using TaxiNT.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TaxiNT.Client.Services.Interfaces;
using TaxiNT.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// UI: Tăng kích thước bộ nhớ đệm
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

// API: Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// API: call HTTP client Hub để lấy dữ liệu API từ bên ngoài
builder.Services.AddHttpClient();

// UI: Get httpClient API default
builder.Services.AddScoped(
    defaultClient => new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["API:Hosting"] ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !"))
    });

// API: Add SwaggerGen (dotnet add package Swashbuckle.AspNetCore)
builder.Services.AddSwaggerGen(
    opt =>
    {
        /*
        //Fix Identity, Refresh Token, Access Token, 
        //CURD Product, Editor Text, Review, Blog, Image Upload, Login API Facebook, Google
        //AugCenterLibrary
        opt.SwaggerDoc("v3", new OpenApiInfo { Title = "AugCenter API", Version = "v3" });
        //For Identity, Login, Register, Change Mudblazor, AugCenterLog, AugCenterDocs
        opt.SwaggerDoc("v2", new OpenApiInfo { Title = "AugCenter API", Version = "v2" });
        */
        //Init project: CRUD category,order,orderdetail,..., AugCenterModel
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Blazor Server Core API", Version = "v1" });
        opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")"
        });

        //Add filter to block case authorize: Swashbuckle.AspNetCore.Filters
        opt.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

// API: Register Server Services
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderByHistoryService, OrderByHistoryService>();
builder.Services.AddScoped<ISalaryAPIService, SalaryAPIService>();

// UI: Register Client Services
builder.Services.AddScoped<ICheckerService, CheckerService>();
builder.Services.AddScoped<ICheckerDetailService, CheckerDetailService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else // API: Add run Swagger UI: https://localhost:7154/swagger/index.html
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(
        opt =>
        {
            opt.SwaggerEndpoint($"/swagger/v1/swagger.json", "Manager Business API V1");
        }
    );
}

app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();
app.MapControllers();
app.UseAntiforgery();

// Xữ lý Header: 
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Remove("X-Frame-Options");
        context.Response.Headers.Remove("Content-Security-Policy");
        context.Response.Headers["Content-Security-Policy"] = "frame-ancestors *"; //Cho phép nhung iframe từ bất kỳ nguồn nào
        return Task.CompletedTask;
    });

    await next();
});

// API: Add Authoz and Authen
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TaxiNT.Client._Imports).Assembly);

app.Run();
