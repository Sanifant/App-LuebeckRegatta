using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();

        DataContext = App.Services.GetService<SettingsViewModel>();
    }
}