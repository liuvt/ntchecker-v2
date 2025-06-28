using Microsoft.Extensions.Logging;
using TaxiNT.MAUI.Services;
using TaxiNT.MAUI.Services.Interfaces;

namespace TaxiNT.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // UI: Get httpClient API default
            builder.Services.AddScoped(
                defaultClient => new HttpClient
                {
                    BaseAddress = new Uri("http://checker.tryasp.net/")
                });

            // UI: Register Client Services
            builder.Services.AddScoped<ICheckerService, CheckerService>();
            builder.Services.AddScoped<ICheckerDetailService, CheckerDetailService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
