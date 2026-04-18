using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        var viewModel = App.Services.GetService<SettingsViewModel>();
        if (viewModel is null)
        {
            throw new System.InvalidOperationException($"{nameof(SettingsViewModel)} is not registered in the application service provider.");
        }
        DataContext = viewModel;
    }
}