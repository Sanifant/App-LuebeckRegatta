using LuebeckRegatta.App.ViewModels;

namespace LuebeckRegatta.App.Controls;

public partial class FlyoutHeader : ContentView
{
	public FlyoutHeader()
	{
		InitializeComponent();
		BindingContext = new FlyoutHeaderViewModel();
	}
}