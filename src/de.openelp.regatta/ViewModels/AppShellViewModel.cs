using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace de.openelp.regatta.ViewModels;

/// <summary>
/// Root-ViewModel, hält die aktuell angezeigte View (VM) und steuert Navigation.
/// </summary>
public partial class AppShellViewModel : ViewModelBase
{
    private object? _current;

    public AppShellViewModel(
        RefereeDashboardViewModel refereeDashboardViewModel,
        SettingsViewModel settingsViewModel)
    {
        RefereeDashboard = refereeDashboardViewModel;
        Settings = settingsViewModel;

        Current = RefereeDashboard;
    }

    public RefereeDashboardViewModel RefereeDashboard { get; }
    public SettingsViewModel Settings { get; }

    public object? Current
    {
        get => _current;
        set
        {
            if (ReferenceEquals(_current, value)) return;
            _current = value;
            OnPropertyChanged();
        }
    }

[RelayCommand]
    public void NavigateToRefereeDashboard()
    {
        Current = RefereeDashboard;
    }

    [RelayCommand]
    public void NavigateToSettingsCommand() { 
        Current = Settings;
     }
}