using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace de.openelp.regatta.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage = "Willkommen zur Lübeck Regatta " + DateTime.Now.Year + "!";

    public HomeViewModel()
    {
    }
}