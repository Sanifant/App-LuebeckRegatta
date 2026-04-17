using Avalonia.Controls;
using de.openelp.regatta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace de.openelp.regatta.Views;

public partial class RefereeDashboardView : ContentPage
{
    public RefereeDashboardView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<RefereeDashboardViewModel>();
    }
}