using LuebeckRegatta.App.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LuebeckRegatta.App.ViewModels;

public class NewsDetailViewModel : INotifyPropertyChanged
{
    private NewsItem _newsItem;

    public event PropertyChangedEventHandler? PropertyChanged;

    public NewsItem NewsItem
    {
        get => _newsItem;
        set
        {
            _newsItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasImage));
        }
    }

    public bool HasImage => !string.IsNullOrEmpty(NewsItem?.ImageUrl);

    public ICommand OpenLinkCommand { get; }

    public NewsDetailViewModel()
    {
        _newsItem = new NewsItem();
        OpenLinkCommand = new Command(async () => await OpenLink());
    }

    private async Task OpenLink()
    {
        if (!string.IsNullOrEmpty(NewsItem?.Link))
        {
            try
            {
                await Browser.OpenAsync(NewsItem.Link, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Öffnen des Links: {ex.Message}");
            }
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}