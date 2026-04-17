using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();

        DataContext = App.Services.GetService<HomeViewModel>();
    }
}