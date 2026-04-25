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

    public string AppVersion => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? "1.0.0";

    public string? Copyright => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyCopyrightAttribute>()?
        .Copyright;

    public string? Description => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyDescriptionAttribute>()?
        .Description;

    public string? ProductTitle => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyProductAttribute>()?
        .Product;

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
        Pages.Add(new PageItem("Home", () => new HomeView(), "M10,20 L30,20 L30,40 L10,40 Z"));
        Pages.Add(new PageItem("Wettkampfrichter", () => new RefereeDashboardView(), "M12,2L4.5,20.29L5.21,21L12,18L18.79,21L19.5,20.29L12,2Z"));
        Pages.Add(new PageItem("Einstellungen", () => new SettingsPage(), "M10,20 L30,20 L30,40 L10,40 Z"));

        NavigateTo(0);
    }
}