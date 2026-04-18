using Avalonia.Controls;
using de.openelp.regatta.ViewModels;
using System;

namespace de.openelp.regatta.Views;

public partial class MainView : DrawerPage
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel vm)
        {
            vm.Navigator = NavPage;
        }
    }
}