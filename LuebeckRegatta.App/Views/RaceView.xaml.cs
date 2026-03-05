using LuebeckRegatta.App.ViewModels;

namespace LuebeckRegatta.App.Views;

public partial class RaceView : ContentPage
{
	public RaceView()
	{
		InitializeComponent();
		BindingContext = new RaceViewModel();
	}
}