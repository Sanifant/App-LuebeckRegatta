using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Avalonia.Android;
using de.openelp.regatta;
using de.openelp.regatta.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;

namespace de.openelp.luebeckregatta.android;

[Activity(
    Label = "de.openelp.regatta.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
    private const string ConfigName = "de.openelp.lrv";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        var config = App.Services.GetRequiredService<IAppConfiguration>();
        var preferences = GetSharedPreferences(ConfigName, FileCreationMode.Private);

        if(preferences != null &&preferences.Contains("WebApiBaseUrl"))
        {
            config.WebApiBaseUrl = preferences.GetString("WebApiBaseUrl", string.Empty) ?? string.Empty;
            config.SelectedEventId = preferences.GetInt("SelectedEvent", -1);
            config.UserName = preferences.GetString("UserName", string.Empty) ?? string.Empty;
            config.Password = preferences.GetString("Password", string.Empty) ?? string.Empty;
        }
    }

    protected override void OnPause()
    {
        base.OnPause();

        var config = App.Services.GetRequiredService<IAppConfiguration>();
        var preferences = GetSharedPreferences(ConfigName, FileCreationMode.Private);

        if (preferences != null)
        {
            preferences.Edit()
            .PutString("WebApiBaseUrl", config.WebApiBaseUrl)
            .PutInt("SelectedEvent", config.SelectedEventId)
            .PutString("UserName", config.UserName)
            .PutString("Password", config.Password)
            .Apply();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}