using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta.Pages;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();

        var homeViewModel = App.Services.GetService<HomeViewModel>();
        if (homeViewModel is null)
        {
            throw new System.InvalidOperationException($"{nameof(HomeViewModel)} is not registered in the application service provider.");
        }
        DataContext = homeViewModel;
    }
}