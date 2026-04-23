using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace de.openelp.regatta.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
	private readonly IAppConfiguration _configuration;
    private ObservableCollection<EventModel>? availableEvents;

    public SettingsViewModel(IAppConfiguration? configuration = null)
	{
		_configuration = configuration ?? AppConfiguration.Current;
		availableEvents = new ObservableCollection<EventModel>();
		this.AvailableThemes = new ObservableCollection<string>(){ "Light", "Dark", "System" };

		if (!String.IsNullOrWhiteSpace(configuration.WebApiBaseUrl)) LoadEvents();

        UrlValid = "OrangeRed";
    }

	public bool CanCheckConnection
	{
		get
		{
			return !string.IsNullOrWhiteSpace(WebApiBaseUrl) 
				&& !string.IsNullOrWhiteSpace(UserName) 
				&& !string.IsNullOrWhiteSpace(Password);
        }
	}

	[RelayCommand]
	public void CheckConnection()
	{
		var boatApiService = new BoatApiService(this._configuration);

        this.CanSaveChanges = true;
	}

    
	public bool CanSaveChanges { get; set; }

	public string UrlValid { get; set; }


    [RelayCommand]
	public Task SaveChanges()
	{
		
		return Task.CompletedTask;
	}	

	private async void LoadEvents()
	{
		var _eventApiService = new EventApiService(this._configuration);
		try
		{
			this.availableEvents = new ObservableCollection<EventModel>(await _eventApiService.GetEventsAsync());
            UrlValid = "Green";
            OnPropertyChanged(nameof(AvailableEvents));
		}
		catch (HttpRequestException ex)
		{
			UrlValid = "OrangeRed";			
		}
		
    }

    public string WebApiBaseUrl
	{
		get => _configuration.WebApiBaseUrl;
		set
		{
		if (_configuration.WebApiBaseUrl == value)
			{
				return;
			}

			_configuration.WebApiBaseUrl = value;
            OnPropertyChanged(nameof(CanCheckConnection));
            LoadEvents();
			OnPropertyChanged();
		}
	}

	public ObservableCollection<string> AvailableThemes { get; }

	
	public ObservableCollection<EventModel> AvailableEvents {
		get
		{
			if (availableEvents == null)
			{
				LoadEvents();
			}
			return availableEvents; 
		}
	}

    public EventModel SelectedEvent
	{
		get => _configuration.SelectedEvent;
		set
		{
			if (_configuration.SelectedEvent == value)
			{
				return;
			}

			_configuration.SelectedEvent = value;
			OnPropertyChanged();
		}
	}

	public string SelectedTheme
	{
		get => _configuration.AppTheme;
		set
		{
			if (_configuration.AppTheme == value)
			{
				return;
			}

			_configuration.AppTheme = value;
			OnPropertyChanged();
		}
	}

	public string UserName
	{
		get => _configuration.UserName;
		set
		{
			if (_configuration.UserName == value)
			{
				return;
			}

			_configuration.UserName = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(CanCheckConnection));
		}
	}
	
	public string Password
	{
		get => _configuration.Password;
		set
		{
			if (_configuration.Password == value)
			{
				return;
			}

			_configuration.Password = value;
			OnPropertyChanged();
            OnPropertyChanged(nameof(CanCheckConnection));
        }
	}

	public bool IsDebugMode
	{
		get => _configuration.IsDebugMode;
		set
		{
			if (_configuration.IsDebugMode == value)
			{
				return;
			}

			_configuration.IsDebugMode = value;
			OnPropertyChanged();
		}
	}
}