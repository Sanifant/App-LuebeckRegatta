using LuebeckRegatta.App.ViewModels;

namespace LuebeckRegatta.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel();
    }
}
