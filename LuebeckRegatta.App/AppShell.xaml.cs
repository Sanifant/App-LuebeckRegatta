using LuebeckRegatta.App.Views;

namespace LuebeckRegatta.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Route für die Detail-Seite registrieren
        Routing.RegisterRoute(nameof(NewsDetailPage), typeof(NewsDetailPage));
    }
}
