using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta;

public partial class PublicBoatView : ContentPage
{
    public PublicBoatView()
    {
        InitializeComponent();

        var viewModel = App.Services.GetService<PublicBoatViewModel>();
        if (viewModel is null)
        {
            throw new System.InvalidOperationException($"{nameof(PublicBoatViewModel)} is not registered in the application service provider.");
        }
        DataContext = viewModel;
    }
}