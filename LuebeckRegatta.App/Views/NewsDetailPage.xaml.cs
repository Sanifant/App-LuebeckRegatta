using LuebeckRegatta.App.ViewModels;

namespace LuebeckRegatta.App.Views;

public partial class NewsDetailPage : ContentPage
{
    public NewsDetailPage(NewsDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}