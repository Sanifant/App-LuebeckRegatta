using LuebeckRegatta.App.Models;
using LuebeckRegatta.App.Repositories;
using LuebeckRegatta.App.Services;
using LuebeckRegatta.App.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LuebeckRegatta.App.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly INewsRepository _newsRepository;
        private bool _isLoading;
        private bool _isRefreshing;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<NewsItem> NewsItems { get; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ItemTappedCommand { get; }

        public MainPageViewModel()
        {
            var settingsService = new SettingsService();
            _newsRepository = new NewsRepository(settingsService);
            RefreshCommand = new Command(async () => await RefreshAsync());
            ItemTappedCommand = new Command<NewsItem>(async (item) => await NavigateToDetail(item));
            
            _ = LoadNewsAsync();
        }

        public async Task LoadNewsAsync()
        {
            IsLoading = true;
            try
            {
                var feed = await _newsRepository.GetNewsFeedAsync();
                if (feed?.Items != null)
                {
                    NewsItems.Clear();
                    foreach (var item in feed.Items)
                    {
                        NewsItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der News: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadNewsAsync();
            IsRefreshing = false;
        }

        private async Task NavigateToDetail(NewsItem newsItem)
        {
            if (newsItem == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "NewsItem", newsItem }
            };

            await Shell.Current.GoToAsync(nameof(NewsDetailPage), parameters);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
