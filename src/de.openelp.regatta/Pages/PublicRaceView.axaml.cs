using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta;

public partial class PublicRaceView : ContentPage
{
    public PublicRaceView()
    {
        InitializeComponent();

        var viewModel = App.Services.GetService<PublicRaceViewModel>();
        if (viewModel is null)
        {
            throw new System.InvalidOperationException($"{nameof(PublicRaceViewModel)} is not registered in the application service provider.");
        }
        DataContext = viewModel;
    }
}