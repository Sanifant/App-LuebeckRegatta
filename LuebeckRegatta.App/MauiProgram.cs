using Microsoft.Extensions.Logging;
using LuebeckRegatta.App.Repositories;
using LuebeckRegatta.App.ViewModels;
using LuebeckRegatta.App.Views;
using CommunityToolkit.Maui;

namespace LuebeckRegatta.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Services registrieren
            builder.Services.AddSingleton<INewsRepository, NewsRepository>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<NewsDetailViewModel>();
            builder.Services.AddTransient<NewsDetailPage>();

            return builder.Build();
        }
    }
}
