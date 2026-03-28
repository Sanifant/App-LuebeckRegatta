using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using de.openelp.regatta;

namespace de.openelp.luebeckregatta.android;

[Activity(
    Label = "de.openelp.regatta.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}