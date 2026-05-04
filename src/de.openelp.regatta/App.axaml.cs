using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Services;
using de.openelp.regatta.ViewModels;
using de.openelp.regatta.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace de.openelp.regatta;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = default!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var appConfiguration = AppConfiguration.Current;

        // For Desktop: load persisted configuration before building services
        FileConfigurationPersistence? persistence = null;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
        {
            persistence = new FileConfigurationPersistence();
            persistence.LoadAsync(appConfiguration).GetAwaiter().GetResult();
        }

        var webApiBaseUrlOverride = Environment.GetEnvironmentVariable("REGATTA_WEB_API_BASE_URL");
        if (!string.IsNullOrWhiteSpace(webApiBaseUrlOverride))
        {
            appConfiguration.WebApiBaseUrl = webApiBaseUrlOverride;
        }

        ReloadServices(persistence);

        var vm = Services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Block the shutdown handler so the config is fully saved before the process exits.
            desktop.ShutdownRequested += (_, _) =>
                persistence!.SaveAsync(AppConfiguration.Current).GetAwaiter().GetResult();

            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Rebuilds the DI service container, optionally registering the given configuration persistence implementation.
    /// </summary>
    /// <param name="configurationPersistence">
    /// The persistence service to register, or <c>null</c> if no persistence is needed
    /// (e.g. on Android where <c>MainActivity</c> handles it).
    /// </param>
    public void ReloadServices(IConfigurationPersistence? configurationPersistence = null)
    {
        var collection = new ServiceCollection();
        var appConfiguration = AppConfiguration.Current;
        collection.AddSingleton<IAppConfiguration>(appConfiguration);

        if (configurationPersistence != null)
        {
            collection.AddSingleton<IConfigurationPersistence>(configurationPersistence);
        }

        collection.AddSingleton<IEventApiService, EventApiService>();
        collection.AddSingleton<IRefereeApiService, RefereeApiService>();
        collection.AddSingleton<IRaceHeatApiService, RaceHeatApiService>();
        collection.AddSingleton<IRaceApiService, RaceApiService>();
        collection.AddSingleton<IBoatApiService, BoatApiService>();
        collection.AddSingleton<IAuthApiService, AuthApiService>();

        collection.AddTransient<MainViewModel>();
        collection.AddTransient<RefereeDashboardViewModel>();
        collection.AddTransient<SettingsViewModel>();
        collection.AddTransient<HomeViewModel>();
        collection.AddTransient<PublicBoatViewModel>();
        collection.AddTransient<PublicRaceViewModel>();

        Services = collection.BuildServiceProvider();
    }
}