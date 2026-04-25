using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using de.openelp.regatta.Models;
using de.openelp.regatta.Views;
using de.openelp.regatta.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace de.openelp.regatta.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _selectedPageIndex;

    [ObservableProperty]
    private bool _isDrawerOpened;

    [ObservableProperty]
    private string _version = "Regatta App (c) " + Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    public ObservableCollection<PageItem> Pages { get; } = new();

    public INavigation? Navigator { get; internal set; }

    public int SelectedPageIndex
    {
        get { return _selectedPageIndex; }
        set
        {
            _selectedPageIndex =  value;
            NavigateTo(_selectedPageIndex);

            IsDrawerOpened = false;
        }
    }

    private Task NavigateTo(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= Pages.Count || Navigator is null)
            return Task.CompletedTask;

        var item = Pages[pageIndex];

        if (item != null)
        {
            var view = item.Factory();
            if (view is Page page && view.GetType() != Navigator.NavigationStack.LastOrDefault()?.GetType())
            {
                return Navigator.ReplaceAsync(page);
            }
        }
        return Task.CompletedTask;
    }

    public MainViewModel()
    {
        Pages.Add(new PageItem("Home", () => new HomeView(), Icons.Bell));
        Pages.Add(new PageItem("Rennfolge", () => new PublicRaceView(), Icons.Clock));
        Pages.Add(new PageItem("Boote", () => new PublicBoatView(), Icons.Monitor));
        Pages.Add(new PageItem("Wettkampfrichter", () => new RefereeDashboardView(), Icons.Navigation, () => false));
        Pages.Add(new PageItem("Einstellungen", () => new SettingsPage(), Icons.Tag));
        this.SelectedPageIndex = 0;
    }
}