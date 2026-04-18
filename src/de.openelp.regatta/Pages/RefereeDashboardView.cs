using Avalonia.Controls;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta.Pages;

public partial class RefereeDashboardView : ContentPage
{
    public RefereeDashboardView()
    {
        InitializeComponent();

        var viewModel = App.Services.GetService<RefereeDashboardViewModel>();
        if (viewModel is null)
        {
            throw new System.InvalidOperationException($"{nameof(RefereeDashboardViewModel)} is not registered in the application service provider.");
        }
        DataContext = viewModel;
    }
}